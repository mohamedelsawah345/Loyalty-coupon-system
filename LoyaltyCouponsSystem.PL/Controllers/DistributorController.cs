using DocumentFormat.OpenXml.InkML;
using LoyaltyCouponsSystem.BLL.Service.Abstraction;
using LoyaltyCouponsSystem.BLL.Service.Implementation;
using LoyaltyCouponsSystem.BLL.ViewModel.Distributor;
using LoyaltyCouponsSystem.DAL.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.PL.Controllers
{
    public class DistributorController : Controller
    {
        private readonly IDistributorService _distributorService;
        private readonly ICustomerService _customerService;
        private readonly ApplicationDbContext _DBcontext;
        private readonly ILogger<DistributorController> _logger;

        public DistributorController(ApplicationDbContext context, IDistributorService distributorService, ICustomerService customerService, ILogger<DistributorController> logger)
        {
            _DBcontext = context;
            _distributorService = distributorService;
            _customerService = customerService;
            _logger = logger;
        }
        [Authorize(Policy = "Manage Customers")]
        public IActionResult Index()
        {
            return View();
        }

        // Add Distributor (GET)
        [HttpGet]
        public async Task<IActionResult> AddDistributor()
        {
            var viewModel = new DistributorViewModel
            {
                Governates = (List<SelectListItem>)await _distributorService.GetGovernatesForDropdownAsync(),
                Customers = (await _customerService.GetAllAsync())
                    .Where(c => c.IsActive) // Only include active customers
                    .Select(c => new SelectListItem
                    {
                        Value = c.CustomerID.ToString(),
                        Text = $"{c.Name} - {c.Code}"
                    })
                    .ToList(),
                SelectedCustomerNames = new List<string>()  // Ensure initialization to avoid null
            };

            return View(viewModel);
        }

        // Add Distributor (POST)
        [HttpPost]
        public async Task<IActionResult> AddDistributor(DistributorViewModel model)
        {
            // Manually validate the uniqueness of the phone number
            var phoneValidationResult = await ValidatePhoneNumberUniqueness(model.PhoneNumber1);

            if (phoneValidationResult != null)
            {
                // If the phone number is not unique, add the error to ModelState
                ModelState.AddModelError("PhoneNumber", phoneValidationResult);
            }

            // Manually validate the uniqueness of the code
            var codeValidationResult = await ValidateCodeUniqueness(model.Code);

            if (codeValidationResult != null)
            {
                // If the code is not unique, add the error to ModelState
                ModelState.AddModelError("Code", codeValidationResult);
            }

           

            // Check if ModelState is valid after all manual validations
            if (ModelState.IsValid)
            {
                var result = await _distributorService.AddAsync(model);
                if (result)
                {
                    return RedirectToAction("GetAllDistributors", "Distributor");
                }

                // If Add fails, show error and re-render
                ModelState.AddModelError("", "Unable to add distributor. Please try again.");
            }

            // If validation fails, re-populate dropdowns and return the view
            model.Governates = (List<SelectListItem>)await _distributorService.GetGovernatesForDropdownAsync();
            model.Customers = (await _customerService.GetAllAsync())
                .Where(c => c.IsActive)
                .Select(c => new SelectListItem
                {
                    Value = c.CustomerID.ToString(),
                    Text = $"{c.Name} - {c.Code}"
                })
                .ToList();
            model.SelectedCustomerNames ??= new List<string>();  // Ensure it's initialized if null

            return View(model);
        }

        private async Task<string> ValidatePhoneNumberUniqueness(string phoneNumber)
        {
            var normalizedPhoneNumber = phoneNumber;

            // Check uniqueness for Distributor
            var existsDistributor = await _DBcontext.Distributors
                .AnyAsync(d => d.PhoneNumber1 == normalizedPhoneNumber);

            if (existsDistributor)
            {
                return "The phone number is already in use for a distributor.";
            }

            // Return null if the phone number is unique
            return null;
        }

        private async Task<string> ValidateCodeUniqueness(string code)
        {
            var normalizedCode = code?.Trim().ToLower();

            // Check uniqueness for Distributor
            var existsDistributor = await _DBcontext.Distributors
                .AnyAsync(d => EF.Functions.Like(d.Code, normalizedCode));

            if (existsDistributor)
            {
                return "The code is already in use for a distributor.";
            }

            // Return null if the code is unique
            return null;
        }

       


        // Update Distributor (GET)
        [HttpGet]
        public async Task<IActionResult> UpdateDistributor(int id)
        {
            // Validate the id
            if (id <= 0)
            {
                return NotFound();
            }

            var distributorViewModel = await _distributorService.GetByIdAsync(id);
            if (distributorViewModel == null)
            {
                return NotFound();
            }

            distributorViewModel.Governates = (List<SelectListItem>)await _distributorService.GetGovernatesForDropdownAsync();
            distributorViewModel.Customers = (await _customerService.GetAllAsync())
                .Where(c => c.IsActive)
                .Select(c => new SelectListItem
                {
                    Value = c.CustomerID.ToString(),
                    Text = $"{c.Name} ({c.Code})"
                })
                .ToList();
            distributorViewModel.SelectedCustomerNames ??= new List<string>();  // Ensure it's initialized

            return View(distributorViewModel); // Return populated ViewModel to the view
        }

        // Update Distributor (POST)
        [HttpPost]
        public async Task<IActionResult> UpdateDistributor(UpdateVM distributorViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _distributorService.UpdateAsync(distributorViewModel);
                if (result)
                {
                    return RedirectToAction("GetAllDistributors"); // Redirect to list after successful update
                }

                ModelState.AddModelError("", "Unable to update distributor. Please try again.");
            }

            // If update fails, re-populate dropdowns and return the view again
            distributorViewModel.Customers = (await _customerService.GetAllAsync())
                .Where(c => c.IsActive)
                .Select(c => new SelectListItem
                {
                    Value = c.CustomerID.ToString(),
                    Text = $"{c.Name} ({c.Code})"
                })
                .ToList();
            distributorViewModel.SelectedCustomerNames ??= new List<string>();  // Ensure it's initialized

            return View(distributorViewModel);
        }

        // Get All Distributors
        [HttpGet]
        public async Task<IActionResult> GetAllDistributors()
        {
            var distributors = await _distributorService.GetAllAsync();
            return View(distributors);
        }

        // Delete Distributor (POST)
        [HttpPost]
        public async Task<IActionResult> DeleteDistributor(int id)
        {
            var result = await _distributorService.DeleteAsync(id);
            if (result)
            {
                return RedirectToAction("GetAllDistributors");
            }

            ModelState.AddModelError("", "Unable to delete distributor. Please try again.");
            return RedirectToAction("GetAllDistributors");
        }

        // Toggle Distributor Activation
        [HttpPost]
        public async Task<IActionResult> ToggleActivation(int distributorId)
        {
            var distributor = await _DBcontext.Distributors.FirstOrDefaultAsync(d => d.DistributorID == distributorId);
            if (distributor == null)
            {
                return Json(new { success = false, message = "Distributor not found." });
            }

            distributor.IsActive = !distributor.IsActive;
            distributor.UpdatedAt = DateTime.Now;
            distributor.UpdatedBy = User.Identity.Name ?? "Unknown"; // Assuming authentication is enabled

            await _DBcontext.SaveChangesAsync();

            return Json(new { success = true, isActive = distributor.IsActive });
        }

        // Import Distributors from Excel
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
                var result = await _distributorService.ImportDistributorsFromExcelAsync(stream);

                if (result)
                {
                    TempData["SuccessMessage"] = "Distributor imported successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to import Distributor. Please check the file format.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("GetAllDistributors");
        }

        // Add Customer to Distributor
        [HttpPost]
        public async Task<IActionResult> AddCustomer(int distributorId, int customerId)
        {
            var success = await _distributorService.AddCustomerToDistributorAsync(distributorId, customerId);

            if (success)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCustomer(int distributorId, string name)
        {
            // Call the service method using the distributor ID and customer name
            var success = await _distributorService.RemoveCustomerFromDistributorByNameAsync(distributorId, name);

            // Return the result as JSON
            return Json(new { success });
        }


    }
}
