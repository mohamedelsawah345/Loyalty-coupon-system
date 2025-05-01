using LoyaltyCouponsSystem.BLL.ViewModel.Customer;
using LoyaltyCouponsSystem.BLL.ViewModel.Distributor;
using LoyaltyCouponsSystem.BLL.ViewModel.ReceiveFromCustomer;
using LoyaltyCouponsSystem.BLL.ViewModel.Technician;
using LoyaltyCouponsSystem.DAL.DB;
using LoyaltyCouponsSystem.DAL.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyCouponsSystem.PL.Controllers
{
    public class ReceiveFromCustomerController : Controller
    {
        private readonly ApplicationDbContext _DBcontext;

        public ReceiveFromCustomerController(ApplicationDbContext context)
        {
            _DBcontext = context;
        }
        [HttpGet]
        public async Task<IActionResult> ReturnItems()
        {
            // Prepare ViewBag for dropdowns
            ViewBag.Customers = await _DBcontext.Customers
                .Where(c => c.IsActive)
                .Select(c => new CustomerViewModel
                {
                    CustomerID = c.CustomerID,
                    Name = c.Name,
                    Code = c.Code
                })
                .ToListAsync();

            ViewBag.Distributors = await _DBcontext.Distributors
                .Where(d => d.IsActive)
                .Select(d => new DistributorViewModel
                {
                    DistributorID = d.DistributorID,
                    Name = d.Name,
                    Code = d.Code
                })
                .ToListAsync();

            ViewBag.Technicians = await _DBcontext.Technicians
                .Where(t => t.IsActive)
                .Select(t => new TechnicianViewModel
                {
                    TechnicianID = t.TechnicianID,
                    Name = t.Name,
                    Code = t.Code
                })
                .ToListAsync();

            // Fetch Governorates only
            ViewBag.Governorates = await _DBcontext.Governorates
                .Select(g => new { g.Id, g.Name })
                .ToListAsync();

            return View(new ReceiveFromCustomerViewModel());
        }
        [HttpGet("GetAreasByGovernorate/{governorateId}")]
        public async Task<IActionResult> GetAreasByGovernorate(int governorateId)
        {
            Console.WriteLine($"Governorate ID Received: {governorateId}");

            // Fetch all areas and print them
            var allAreas = await _DBcontext.Areas.ToListAsync();
            Console.WriteLine($"Total Areas in DB: {allAreas.Count}");

            var areas = await _DBcontext.Areas
                .Where(a => a.GovernateId == governorateId) // Ensure correct property name
                .Select(a => new { a.Id, a.Name })
                .ToListAsync();


            if (!areas.Any())
            {
                return NotFound($"No areas found for governorate ID: {governorateId}");
            }

            return Ok(areas);
        }






        [HttpPost]
        public async Task<IActionResult> ReturnItems(ReceiveFromCustomerViewModel model)
        {
            // Check if the coupon receipt number is unique

            try
            {
                // Create and save the transaction
                var transaction = new ReceiveFromCustomer
                {
                    CustomerCode = _DBcontext.Customers.Where(c => c.CustomerID == model.CustomerId).Select(c => c.Code).FirstOrDefault(),
                    DistributorCode = _DBcontext.Distributors.Where(d => d.DistributorID == model.DistributorId).Select(d => d.Code).FirstOrDefault(),
                    TechnicianCode = _DBcontext.Technicians.Where(t => t.TechnicianID == model.TechnicianId).Select(t => t.Code).FirstOrDefault(),
                    GovernorateId = model.GovernorateId,
                    Governorates = _DBcontext.Governorates.FirstOrDefault(g => g.Id == model.GovernorateId),
                    AreaId = model.AreaId,
                    Areas = _DBcontext.Areas.FirstOrDefault(a => a.Id == model.AreaId),
                    CouponReceiptNumber = model.CouponReceiptNumber,
                    TransactionDate = DateTime.Now
                };

            var existingTransaction = await _DBcontext.ReceiveFromCustomers
                .FirstOrDefaultAsync(t => t.CouponReceiptNumber == model.CouponReceiptNumber);

            if (existingTransaction != null)
            {
                TempData["Error"] = "The coupon receipt number is already in use. Please provide a unique receipt number.";
                return RedirectToAction("ReturnItems");
            }
                _DBcontext.ReceiveFromCustomers.Add(transaction);
                await _DBcontext.SaveChangesAsync();

                TempData["Success"] = "Transaction submitted successfully!";
            }
            catch (Exception ex)
            {
                var customerExists = _DBcontext.Customers.Any(c => c.CustomerID == model.CustomerId);
                if (!customerExists)
                {
                    TempData["Error"] = "Invalid CustomerId. Please select a valid customer.";
                    return RedirectToAction("ReturnItems");
                }
                var areaExists = _DBcontext.Areas.Any(a => a.Id == model.AreaId);
                if (!areaExists)
                {
                    TempData["Error"] = "Invalid AreaId. Please select a valid area.";
                    return RedirectToAction("ReturnItems");
                }
                TempData["Error"] = "An error occurred while submitting the transaction. Please try again.";
            }

            return RedirectToAction("AllTransactions");
        }


        public async Task<IActionResult> AllTransactions(int page = 1, int pageSize = 10)
        {
            var totalTransactions = await _DBcontext.ReceiveFromCustomers
                .CountAsync();

            var transactions = await _DBcontext.ReceiveFromCustomers
                .Include(t => t.Governorates)  // Only include related entities
                .Include(t => t.Areas)
                .OrderByDescending(t => t.TransactionDate)  // Latest first
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new ReceiveFromCustomerViewModel
                {
                    CustomerCodeAndName = t.CustomerCode,  // No need to Include here
                    DistributorCodeAndName = t.DistributorCode,
                    TechnicianCodeAndName = t.TechnicianCode,
                    GovernorateName = t.Governorates.Name,
                    AreaName = t.Areas.Name,
                    CouponReceiptNumber = t.CouponReceiptNumber,
                    TransactionDate = t.TransactionDate
                })
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalTransactions / (double)pageSize);

            var model = new TransactionPaginationViewModel
            {
                Transactions = transactions,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(model);
        }

        public async Task<IActionResult> ScannedQrCodes(string receiptNumber)
        {
            if (string.IsNullOrEmpty(receiptNumber))
            {
                TempData["Error"] = "Receipt number is required.";
                return RedirectToAction("AllTransactions", "ReceiveFromCustomer"); // Or another relevant action
            }

            // Fetch QR scan logs based on the receipt number
            var qrScanLogs = await _DBcontext.QRScanLogs
                .Where(log => log.ReceiptNumber == receiptNumber)
                .OrderByDescending(log => log.ScanTime)
                .ToListAsync();

            if (qrScanLogs == null || !qrScanLogs.Any())
            {
                TempData["Error"] = "No QR scan logs found for the provided receipt number.";
                return RedirectToAction("AllTransactions"); // Or another relevant action
            }

            // Get the serial number from the Coupon table
            foreach (var log in qrScanLogs)
            {
                var coupon = await _DBcontext.Coupons
                    .FirstOrDefaultAsync(c => c.CouponeId == log.QR_ID);

                if (coupon != null)
                {
                    log.SerialNumber = coupon.SerialNumber;
                }
                else
                {
                    log.SerialNumber = "Not Found"; // Or any fallback value
                }
            }

            return View(qrScanLogs);
        }


    }
}