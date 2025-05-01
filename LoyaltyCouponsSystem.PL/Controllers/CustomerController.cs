using DocumentFormat.OpenXml.InkML;
using LoyaltyCouponsSystem.BLL.Service.Abstraction;
using LoyaltyCouponsSystem.BLL.ViewModel.Customer;
using LoyaltyCouponsSystem.DAL.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyCouponsSystem.PL.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ApplicationDbContext _DBcontext;

        public CustomerController(ApplicationDbContext context, ICustomerService customerService)
        {
            _DBcontext = context;
            _customerService = customerService;
        }
        [Authorize(Policy = "Manage Customers")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllCustomers()
        {
            var result = await _customerService.GetAllAsync();
            return View(result);
        }

        public async Task<IActionResult> GetCustomerById(int id)
        {
            var result = await _customerService.GetByIdAsync(id);
            return View(result);
        }


        [HttpPost]
        [Route("Customer/ToggleActivation")]
        public async Task<IActionResult> ToggleActivation(int customerId)
        {
            var customer = await _DBcontext.Customers.FindAsync(customerId);
            if (customer == null)
            {
                return Json(new { success = false, message = "Customer not found" });
            }

            // Toggle the IsActive status
            customer.IsActive = !customer.IsActive;
            await _DBcontext.SaveChangesAsync();

            // Return the updated status
            return Json(new { success = true, isActive = customer.IsActive });
        }








        



        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.DeleteAsync(id);
            return RedirectToAction(nameof(GetAllCustomers));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var updateCustomerViewModel = new UpdateCustomerViewModel
            {
                CustomerID = customer.CustomerID,
                Name = customer.Name,
                Code = customer.Code,
                Governate = customer.Governate,
                City = customer.City,
                PhoneNumber = customer.PhoneNumber,
                TechnicianID = customer.TechnicianID,
                IsActive = customer.IsActive // Assuming IsActive is part of the ApplicationUser class

            };

            return View(updateCustomerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerViewModel updateCustomerViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _customerService.UpdateAsync(updateCustomerViewModel);
                if (result)
                {
                    return RedirectToAction("GetAllCustomers");
                }
                ModelState.AddModelError("", "Failed to update customer");
            }

            return View(updateCustomerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a valid Excel file.";
                return RedirectToAction("GetAllCustomers");
            }

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);

                // Process the file in the service
                var result = await _customerService.ImportCustomersFromExcelAsync(stream);

                if (result)
                {
                    TempData["SuccessMessage"] = "Customers imported successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to import customers. Please check the file format.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("GetAllCustomers");
        }

        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCustomer(CustomerViewModel customerViewModel)
        {

            // Manually validate the uniqueness of the phone number
            var phoneValidationResult = await ValidatePhoneNumberUniqueness(customerViewModel.PhoneNumber);

            if (phoneValidationResult != null)
            {
                // If the phone number is not unique, add the error to ModelState
                ModelState.AddModelError("PhoneNumber", phoneValidationResult);
            }

            // Manually validate the uniqueness of the code
            var codeValidationResult = await ValidateCodeUniqueness(customerViewModel.Code);

            if (codeValidationResult != null)
            {
                // If the code is not unique, add the error to ModelState
                ModelState.AddModelError("Code", codeValidationResult);
            }

            if (ModelState.IsValid)
            {
                var result = await _customerService.AddAsync(customerViewModel);
                if (result)
                {
                    return RedirectToAction("GetAllCustomers");
                }
                ModelState.AddModelError("", "Unable to add customer. Please try again.");
            }

            return View(customerViewModel);
        }
        private async Task<string> ValidateCodeUniqueness(string code)
        {
            var normalizedCode = code?.Trim().ToLower();

            // Check uniqueness for Customer
            var existsCustomer = await _DBcontext.Customers
                .AnyAsync(c => EF.Functions.Like(c.Code, normalizedCode));

            if (existsCustomer)
            {
                return "The code is already in use for a customer.";
            }

            // Check uniqueness for Distributor
            var existsDistributor = await _DBcontext.Distributors
                .AnyAsync(d => EF.Functions.Like(d.Code, normalizedCode));

            if (existsDistributor)
            {
                return "The code is already in use for a distributor.";
            }

            // Check uniqueness for Technician
            var existsTechnician = await _DBcontext.Technicians
                .AnyAsync(t => EF.Functions.Like(t.Code, normalizedCode));

            if (existsTechnician)
            {
                return "The code is already in use for a technician.";
            }

            // Return null if the code is unique
            return null;
        }

        private async Task<string> ValidatePhoneNumberUniqueness(string phoneNumber)
        {
            var normalizedPhoneNumber = phoneNumber?.Trim();

            // Check uniqueness for Customer
            var existsCustomer = await _DBcontext.Customers
                .AnyAsync(c => c.PhoneNumber.ToString() == normalizedPhoneNumber);

            if (existsCustomer)
            {
                return "The phone number is already in use for a customer.";
            }

            // Check uniqueness for Distributor
            var existsDistributor = await _DBcontext.Distributors
                .AnyAsync(d => d.PhoneNumber1.ToString() == normalizedPhoneNumber);

            if (existsDistributor)
            {
                return "The phone number is already in use for a distributor.";
            }

            // Check uniqueness for Technician
            var existsTechnician = await _DBcontext.Technicians
                .AnyAsync(t => t.PhoneNumber1.ToString() == normalizedPhoneNumber);

            if (existsTechnician)
            {
                return "The phone number is already in use for a technician.";
            }

            // Return null if the phone number is unique
            return null;
        }


    }

}
