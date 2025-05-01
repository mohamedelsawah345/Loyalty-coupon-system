using DocumentFormat.OpenXml.InkML;
using LoyaltyCouponsSystem.BLL.Service.Abstraction;
using LoyaltyCouponsSystem.BLL.Service.Implementation;
using LoyaltyCouponsSystem.DAL.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyCouponsSystem.PL.Controllers
{
    public class ExchangeOrderController : Controller
    {
        private readonly IExchangeOrderService _service;
        private readonly ApplicationDbContext _context; 

        public ExchangeOrderController(IExchangeOrderService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }
        [Authorize(Policy = "Exchange Permissions")]
        public async Task<IActionResult> GetCustomerDetails(string customerCodeOrName)
        {
            var model = await _service.GetCustomerDetailsAsync(customerCodeOrName);
            return PartialView("_CustomerDetails", model);
        }

        public async Task<IActionResult> GetTechnicianDetails(string technicianCodeOrName)
        {
            var model = await _service.GetTechnicianDetailsAsync(technicianCodeOrName);
            return PartialView("_TechnicianDetails", model);
        }
        public async Task<IActionResult> GetDistributorDetails(string technicianCodeOrName)
        {
            var model = await _service.GetDistributorDetailsAsync(technicianCodeOrName);
            return PartialView("_DistributorDetails", model);
        }
        [HttpGet]
        public async Task<JsonResult> GetCitiesByGovernorate(int governateId)
        {
            var cities = await _service.GetCitiesByGovernateAsync(governateId);
            return Json(cities.Select(city => new { id = city.Value, name = city.Text }));
        }

        [HttpGet]
        public async Task<IActionResult> AssignQRCode()
        {
            var model = await _service.GetAssignmentDetailsAsync();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AssignQRCode(string ExchangePermission, string selectedCustomerCode, string selectedDistributorCode, int? GovernorateId, int? AreaId, List<AssignmentViewModel> transactions)
        {
            var model = await _service.GetAssignmentDetailsAsync();

            // Preserve selected values for user feedback
            model.ExchangePermission = ExchangePermission;
            model.SelectedCustomerCode = selectedCustomerCode;
            model.SelectedDistributorCode = selectedDistributorCode;
            model.SelectedGovernate = GovernorateId?.ToString();
            model.SelectedCity = AreaId?.ToString();

            ServiceToManageStatues serviceToManageStatues = new ServiceToManageStatues(_context);

            // Validate required fields
            if (string.IsNullOrEmpty(selectedCustomerCode))
                ModelState.AddModelError("selectedCustomerCode", "Customer is required.");

            if (string.IsNullOrEmpty(selectedDistributorCode))
                ModelState.AddModelError("selectedDistributorCode", "Distributor is required.");

            if (!GovernorateId.HasValue)
                ModelState.AddModelError("GovernorateId", "Governorate is required.");

            if (!AreaId.HasValue)
                ModelState.AddModelError("AreaId", "City is required.");

            // Validate ExchangePermission uniqueness
            var exchangePermissionValidationResult = await ValidateExchangePermissionUniqueness(ExchangePermission);
            if (exchangePermissionValidationResult != null)
                ModelState.AddModelError("ExchangePermission", exchangePermissionValidationResult);

            foreach (var transaction in transactions)
            {
                // Validate that SequenceStart is greater than the last recorded sequence for this coupon type
                var lastSequence = _context.Transactions
                    .Where(t => t.CouponType == transaction.SelectedCouponType)
                    .OrderByDescending(t => t.TransactionID)
                    .Select(t => t.SequenceEnd)
                    .FirstOrDefault() ?? "0";

                if (string.Compare(transaction.SequenceStart, lastSequence) <= 0)
                {
                    ModelState.AddModelError(nameof(transaction.SequenceStart),
                        $"SequenceStart must be greater than the last recorded sequence number for coupon type {transaction.SelectedCouponType}: {lastSequence}");
                }

                // Update Status, Representative, Customer
                serviceToManageStatues.ManageStatuesEcxhangeOrder(
                    transaction.SequenceStart, transaction.SequenceEnd,
                    selectedCustomerCode, selectedDistributorCode);

                var couponIdentifier = GetCouponIdentifier(transaction.SelectedCouponType);

                // Validate that SequenceStart and SequenceEnd start with the correct prefix
                if (!transaction.SequenceStart.StartsWith(couponIdentifier) ||
                    !transaction.SequenceEnd.StartsWith(couponIdentifier))
                {
                    ModelState.AddModelError(string.Empty,
                        $"Start and End Sequences must start with '{couponIdentifier}' for the selected coupon type.");
                }

                // Ensure unique sequence range
                var isDuplicate = _context.Transactions.Any(t =>
                    t.CouponType == transaction.SelectedCouponType &&
                    string.Compare(t.SequenceStart, transaction.SequenceEnd) <= 0 &&
                    string.Compare(t.SequenceEnd, transaction.SequenceStart) >= 0);

                if (isDuplicate)
                {
                    ModelState.AddModelError(string.Empty,
                        $"The sequence range {transaction.SequenceStart}-{transaction.SequenceEnd} is already used for coupon type {transaction.SelectedCouponType}.");
                }
            }

            // If there are validation errors, return the model with the errors
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Call the service to assign QR codes after validation
            await _service.AssignQRCodeAsync(ExchangePermission, selectedCustomerCode, selectedDistributorCode, GovernorateId.ToString(), AreaId.ToString(), transactions);
            TempData["SuccessMessage"] = "All assignments saved successfully.";

            return RedirectToAction("AllTransactions", "Transaction");
        }


        private async Task<string> ValidateExchangePermissionUniqueness(string exchangePermission)
        {
            if (string.IsNullOrWhiteSpace(exchangePermission))
            {
                return "Exchange Permission is required.";
            }

            var exists = await _context.Transactions
                .AnyAsync(t => t.ExchangePermission == exchangePermission);

            if (exists)
            {
                return $"Exchange Permission Number {exchangePermission} is already used.";
            }

            return null; // Exchange Permission is unique
        }



        // Helper method to map CouponType to the identifier
        private string GetCouponIdentifier(string couponType)
        {
            return couponType switch
            {
                "راك ثيرم" => "1",
                "صرف جي تكس" => "2",
                "اقطار كبيرة وهودذا" => "3",
                "كعب راك ثيرم" => "4",
                "كعب صرف جي تكس" => "5",
                "كعب اقطار كبيرة وهودذا" => "6",
                _ => throw new ArgumentException("Invalid coupon type"),
            };
        }



        // New action for AJAX validation
        [Route("ExchangeOrder/CheckExchangePermission")]
        [HttpPost]
        public async Task<JsonResult> CheckExchangePermission(string exchangePermission)
        {
            try
            {
                var isDuplicate = await _service.IsExchangePermissionDuplicateAsync(exchangePermission);
                return Json(isDuplicate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log the error for debugging
                return Json(false); // Return false to indicate an issue
            }
        }
       

    }
}
