    using LoyaltyCouponsSystem.BLL.Service.Abstraction;
using LoyaltyCouponsSystem.BLL.Service.Implementation;
using LoyaltyCouponsSystem.BLL.ViewModel.Customer;
using LoyaltyCouponsSystem.BLL.ViewModel.Technician;
using LoyaltyCouponsSystem.DAL.DB;
using LoyaltyCouponsSystem.DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System.Text.RegularExpressions;
using ZXing;

namespace LoyaltyCouponsSystem.PL.Controllers
{
    public class TechnicianController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerService _customerService;
        private readonly ITechnicianService _technicianService;
        private readonly ApplicationDbContext _DBcontext;

        public TechnicianController(ApplicationDbContext context, ITechnicianService technicianService, ICustomerService customerService, UserManager<ApplicationUser> userManager)
        {
            _DBcontext = context;
            _technicianService = technicianService;
            _customerService = customerService;
            _userManager = userManager;
        }
        [Authorize(Policy = "Manage Customers")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllTechnicians()
        {
            var technicians = await _technicianService.GetAllAsync();

            foreach (var technician in technicians)
            {
                var dbTechnician = await _DBcontext.Technicians
                    .Include(t => t.TechnicianCustomers) 
                    .ThenInclude(tc => tc.Customer)
                    .Include(t => t.TechnicianUsers)     
                    .ThenInclude(tu => tu.User)          
                    .FirstOrDefaultAsync(t => t.TechnicianID == technician.TechnicianID);

                if (dbTechnician != null)
                {
                    // Set Technician Status
                    technician.IsActive = dbTechnician.IsActive;

                    // Map Active Customers through TechnicianCustomers relation
                    technician.ActiveCustomers = dbTechnician.TechnicianCustomers?
                        .Where(tc => tc.Customer.IsActive)  // Filter only active customers
                        .Select(tc => new CustomerViewModel
                        {
                            CustomerID = tc.Customer.CustomerID,
                            Name = tc.Customer.Name,
                            IsActive = tc.Customer.IsActive
                        })
                        .ToList() ?? new List<CustomerViewModel>();

                    technician.SelectedCustomerNames = technician.ActiveCustomers
                        .Select(c => c.Name)
                        .Distinct()
                        .ToList() ?? new List<string>();

                    // Assign Representatives through TechnicianUsers relation
                    technician.AssignedRepresentatives = dbTechnician.TechnicianUsers?
                        .Where(tu => tu.User.IsActive)  // Filter only active users (representatives)
                        .Select(tu => tu.User)
                        .ToList() ?? new List<ApplicationUser>();

                    technician.SelectedUserNames = technician.AssignedRepresentatives
                        .Select(r => r.UserName)
                        .Distinct()
                        .ToList() ?? new List<string>();
                }

                // Get unassigned customers
                technician.UnassignedActiveCustomers = await _technicianService.GetUnassignedActiveCustomersAsync(technician.TechnicianID);

                // Get active unassigned representatives
                technician.UnassignedActiveUsers = await _technicianService.GetActiveUnassignedRepresentativesAsync(technician.TechnicianID);
            }

            // Get all active representatives
            ViewBag.AllActiveRepresentatives = await _userManager.Users
                .Where(u => u.IsActive)
                .ToListAsync();

            return View(technicians);
        }



        public async Task<IActionResult> GetTechnicianById(int id)
        {
            var result = await _technicianService.GetByIdAsync(id);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> AddTechnician()
        {
            var technicianViewModel = new TechnicianViewModel
            {
                Customers = await _technicianService.GetCustomersForDropdownAsync(),
                Users = await _technicianService.GetUsersForDropdownAsync(),      
                Governates = new List<SelectListItem>
        {
                new SelectListItem { Text = "Aswan", Value = "Aswan" },
                new SelectListItem { Text = "Assiut", Value = "Assiut" },
                new SelectListItem { Text = "Alexandria", Value = "Alexandria" },
                new SelectListItem { Text = "Ismailia", Value = "Ismailia" },
                new SelectListItem { Text = "Luxor", Value = "Luxor" },
                new SelectListItem { Text = "Red Sea", Value = "Red Sea" },
                new SelectListItem { Text = "Beheira", Value = "Beheira" },
                new SelectListItem { Text = "Giza", Value = "Giza" },
                new SelectListItem { Text = "Dakahlia", Value = "Dakahlia" },
                new SelectListItem { Text = "Suez", Value = "Suez" },
                new SelectListItem { Text = "Sharqia", Value = "Sharqia" },
                new SelectListItem { Text = "Gharbia", Value = "Gharbia" },
                new SelectListItem { Text = "Cairo", Value = "Cairo" },
                new SelectListItem { Text = "Qalyubia", Value = "Qalyubia" },
                new SelectListItem { Text = "New Valley", Value = "New Valley" },
                new SelectListItem { Text = "Matrouh", Value = "Matrouh" },
                new SelectListItem { Text = "Minya", Value = "Minya" },
                new SelectListItem { Text = "Fayoum", Value = "Fayoum" },
                new SelectListItem { Text = "Beni Suef", Value = "Beni Suef" },
                new SelectListItem { Text = "Kafr El Sheikh", Value = "Kafr El Sheikh" },
                new SelectListItem { Text = "Port Said", Value = "Port Said" },
                new SelectListItem { Text = "Damietta", Value = "Damietta" },
                new SelectListItem { Text = "Menoufia", Value = "Menoufia" },
                new SelectListItem { Text = "Qena", Value = "Qena" },
                new SelectListItem { Text = "Sohag", Value = "Sohag" },
                new SelectListItem { Text = "South Sinai", Value = "South Sinai" },
                new SelectListItem { Text = "North Sinai", Value = "North Sinai" }
        }
            };
            return View(technicianViewModel);
        }

        // Add Technician (POST)
        [HttpPost]
        public async Task<IActionResult> AddTechnician(TechnicianViewModel model)
        {
            try
            {
                // Manually validate the uniqueness of the phone number
                var phoneValidationResult = await ValidatePhoneNumberUniqueness(model.PhoneNumber1);
                if (phoneValidationResult != null)
                {
                    // If the phone number is not unique, add the error to ModelState
                    ModelState.AddModelError("PhoneNumber1", phoneValidationResult);
                }

                // Manually validate the uniqueness of the code
                var codeValidationResult = await ValidateCodeUniqueness(model.Code);
                if (codeValidationResult != null)
                {
                    // If the code is not unique, add the error to ModelState
                    ModelState.AddModelError("Code", codeValidationResult);
                }

                // Validation checks for mandatory fields
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    ModelState.AddModelError("Name", "Technician name is required.");
                }

                if (string.IsNullOrWhiteSpace(model.PhoneNumber1) || !Regex.IsMatch(model.PhoneNumber1, @"^01\d{9}$"))
                {
                    ModelState.AddModelError("PhoneNumber1", "Please provide a valid phone number.");
                }

                // Additional validations for other phone numbers (if needed)
                if (!string.IsNullOrEmpty(model.PhoneNumber2) && !Regex.IsMatch(model.PhoneNumber2, @"^01\d{9}$"))
                {
                    ModelState.AddModelError("PhoneNumber2", "Please provide a valid phone number.");
                }

                if (!string.IsNullOrEmpty(model.PhoneNumber3) && !Regex.IsMatch(model.PhoneNumber3, @"^01\d{9}$"))
                {
                    ModelState.AddModelError("PhoneNumber3", "Please provide a valid phone number.");
                }

                // Check if ModelState is valid after all manual validations
                if (ModelState.IsValid)
                {
                    var result = await _technicianService.AddAsync(model);
                    if (result)
                    {
                        return RedirectToAction("GetAllTechnicians");
                    }

                    // If Add fails, show error and re-render
                    ModelState.AddModelError("", "Unable to add technician. Please try again.");
                }

                // If validation fails, re-populate dropdowns, governates, and cities
                model.Customers = await _technicianService.GetCustomersForDropdownAsync();
                model.Users = await _technicianService.GetUsersForDropdownAsync();

                // Repopulate the governates list
                model.Governates = new List<SelectListItem>
        {
            new SelectListItem { Text = "Aswan", Value = "Aswan" },
                new SelectListItem { Text = "Assiut", Value = "Assiut" },
                new SelectListItem { Text = "Alexandria", Value = "Alexandria" },
                new SelectListItem { Text = "Ismailia", Value = "Ismailia" },
                new SelectListItem { Text = "Luxor", Value = "Luxor" },
                new SelectListItem { Text = "Red Sea", Value = "Red Sea" },
                new SelectListItem { Text = "Beheira", Value = "Beheira" },
                new SelectListItem { Text = "Giza", Value = "Giza" },
                new SelectListItem { Text = "Dakahlia", Value = "Dakahlia" },
                new SelectListItem { Text = "Suez", Value = "Suez" },
                new SelectListItem { Text = "Sharqia", Value = "Sharqia" },
                new SelectListItem { Text = "Gharbia", Value = "Gharbia" },
                new SelectListItem { Text = "Cairo", Value = "Cairo" },
                new SelectListItem { Text = "Qalyubia", Value = "Qalyubia" },
                new SelectListItem { Text = "New Valley", Value = "New Valley" },
                new SelectListItem { Text = "Matrouh", Value = "Matrouh" },
                new SelectListItem { Text = "Minya", Value = "Minya" },
                new SelectListItem { Text = "Fayoum", Value = "Fayoum" },
                new SelectListItem { Text = "Beni Suef", Value = "Beni Suef" },
                new SelectListItem { Text = "Kafr El Sheikh", Value = "Kafr El Sheikh" },
                new SelectListItem { Text = "Port Said", Value = "Port Said" },
                new SelectListItem { Text = "Damietta", Value = "Damietta" },
                new SelectListItem { Text = "Menoufia", Value = "Menoufia" },
                new SelectListItem { Text = "Qena", Value = "Qena" },
                new SelectListItem { Text = "Sohag", Value = "Sohag" },
                new SelectListItem { Text = "South Sinai", Value = "South Sinai" },
                new SelectListItem { Text = "North Sinai", Value = "North Sinai" }
        };

               

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }


        // Validate Phone Number Uniqueness
        private async Task<string> ValidatePhoneNumberUniqueness(string phoneNumber)
        {
            var normalizedPhoneNumber = phoneNumber;

            // Check uniqueness for Technician
            var existsTechnician = await _DBcontext.Technicians
                .AnyAsync(t => t.PhoneNumber1 == normalizedPhoneNumber);

            if (existsTechnician)
            {
                return "The phone number is already in use for a technician.";
            }

            // Return null if the phone number is unique
            return null;
        }

        // Validate Code Uniqueness
        private async Task<string> ValidateCodeUniqueness(string code)
        {
            var normalizedCode = code?.Trim().ToLower();

            // Check uniqueness for Technician
            var existsTechnician = await _DBcontext.Technicians
                .AnyAsync(t => EF.Functions.Like(t.Code, normalizedCode));

            if (existsTechnician)
            {
                return "The technician code is already in use.";
            }

            // Return null if the code is unique
            return null;
        }


        public async Task<IActionResult> DeleteTechnician(int id)
        {
            await _technicianService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> UpdateTechnician(int id)
        {
            var technician = await _technicianService.GetByIdAsync(id);

            if (technician == null)
            {
                return NotFound();
            }

            var model = new UpdateTechnicianViewModel
            {
                TechnicianID = technician.TechnicianID,
                Code = technician.Code,
                Name = technician.Name,
                NickName = technician.NickName,
                NationalID = technician.NationalID,
                SelectedGovernate = technician.SelectedGovernate,
                PhoneNumber1 = technician.PhoneNumber1,
                PhoneNumber2 = technician.PhoneNumber2,
                PhoneNumber3 = technician.PhoneNumber3,
                SelectedCity = technician.SelectedCity,
                Governates = GetGovernatesList() // Fetch governates dynamically
            }; return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateTechnician(UpdateTechnicianViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _technicianService.UpdateAsync(model);
                if (result)
                {
                    return RedirectToAction("GetAllTechnicians");
                }
                ModelState.AddModelError("", "Failed to update customer");
            }
            return View(model);
        }

        private List<SelectListItem> GetGovernatesList()
        {
            return new List<SelectListItem>
        {
            new SelectListItem { Value = "Aswan", Text = "Aswan" },
            new SelectListItem { Value = "Assiut", Text = "Assiut" },
            new SelectListItem { Value = "Alexandria", Text = "Alexandria" },
            new SelectListItem { Value = "Ismailia", Text = "Ismailia" },
            new SelectListItem { Value = "Luxor", Text = "Luxor" },
            new SelectListItem { Value = "Red Sea", Text = "Red Sea" },
            new SelectListItem { Value = "Beheira", Text = "Beheira" },
            new SelectListItem { Value = "Giza", Text = "Giza" },
            new SelectListItem { Value = "Dakahlia", Text = "Dakahlia" },
            new SelectListItem { Value = "Suez", Text = "Suez" },
            new SelectListItem { Value = "Sharqia", Text = "Sharqia" },
            new SelectListItem { Value = "Gharbia", Text = "Gharbia" },
            new SelectListItem { Value = "Cairo", Text = "Cairo" },
            new SelectListItem { Value = "Qalyubia", Text = "Qalyubia" },
            new SelectListItem { Value = "New Valley", Text = "New Valley" },
            new SelectListItem { Value = "Matrouh", Text = "Matrouh" },
            new SelectListItem { Value = "Minya", Text = "Minya" },
            new SelectListItem { Value = "Fayoum", Text = "Fayoum" },
            new SelectListItem { Value = "Beni Suef", Text = "Beni Suef" },
            new SelectListItem { Value = "Kafr El Sheikh", Text = "Kafr El Sheikh" },
            new SelectListItem { Value = "Port Said", Text = "Port Said" },
            new SelectListItem { Value = "Damietta", Text = "Damietta" },
            new SelectListItem { Value = "Menoufia", Text = "Menoufia" },
            new SelectListItem { Value = "Qena", Text = "Qena" },
            new SelectListItem { Value = "Sohag", Text = "Sohag" },
            new SelectListItem { Value = "South Sinai", Text = "South Sinai" },
            new SelectListItem { Value = "North Sinai", Text = "North Sinai" }
        };
        }

        // Fetch cities based on the selected governate
        public IActionResult GetCitiesByGovernorate(string governorateId)
        {
            var citiesByGovernate = new Dictionary<string, List<string>>
        {
            { "Aswan", new List<string> { "Aswan", "Draou", "Kom Ombo", "Edfu", "Nasser El-Nuba", "Abu Simbel", "Kalabsha", "El-Sebaaia" } },
            { "Assiut", new List<string> { "Assiut", "Dairout", "Manfalut", "Qusiya", "Abnoub", "Abu Teeg", "El-Ghanaim", "Sahil Selim", "Al-Badari", "Seda" } },
            { "Alexandria", new List<string> { "Alexandria", "Borg El Arab", "Al Amiriya", "Sidi Bishr", "Smouha", "El-Montazah", "Stanley", "Miami", "Gleem", "Abu Qir", "Mandara", "El-Max", "Raml Station" } },
            { "Ismailia", new List<string> { "Ismailia", "Fayed", "Qantara Sharq", "Qantara Gharb", "Tel El-Kebir", "Abu Suweir", "Al-Qassasin" } },
            { "Luxor", new List<string> { "Luxor", "Esna", "Armant", "El-Zinia", "El-Bayadiya", "El-Qurna", "El-Tod" } },
            { "Red Sea", new List<string> { "Hurghada", "Ras Ghareb", "Safaga", "El-Quseir", "Marsa Alam", "Shalatin", "Halayeb" } },
            { "Beheira", new List<string> { "Damanhour", "Kafr El-Dawar", "Rashid", "Edko", "Abu Hamsa", "El-Dalangat", "Mahmoudiya", "Itay El-Baroud", "Housh Eissa", "Shobrahet", "Kom Hamada", "Badr", "Wadi El-Natrun", "New Nubaria" } },
            { "Giza", new List<string> { "Giza", "Abu Al-Nemros", "October City", "Sheikh Zayed", "Faisal", "Haram", "Badrashin", "Al-Ayat", "El-Saff", "Atfih", "Wahaat El-Bahareya", "Kirdasa", "Maneshet El-Qanater" } },
            { "Dakahlia", new List<string> { "Mansoura", "Tala", "Mitta Ghamr", "Dekernes", "Aja", "Menya El-Nasser", "El-Senbelawein", "Beni Obaid", "El-Manzala", "Tami El-Omdeed", "El-Matariya", "Balqas", "Meet Salsil", "El-Khordy" } },
            { "Suez", new List<string> { "Suez", "Ain Sokhna", "El-Adabiya" } },
            { "Sharqia", new List<string> { "Zagazig", "Menia El-Qamh", "Belbes", "Mashtool El-Souk", "Abu Hammad", "El-Qanayat", "Hehia", "El-Ibrahimia", "Deirab Najm", "Kafr Saker", "Olad Saker", "Fakous", "El-Salihiya El-Gedida", "Husseiniya", "El-Ashter" } },
            { "Gharbia", new List<string> { "Tanta", "El-Mahalla El-Kubra", "Kafr El-Zayat", "Zefta", "El-Santa", "Khatour", "Basyoun", "Samannoud" } },
            { "Cairo", new List<string> { "Cairo", "Helwan", "Maadi", "Shorouk", "Nasr City", "Fifth Settlement", "Heliopolis", "Zamalek", "Ain Shams", "Sayeda Zainab", "Mousky", "Mohandiseen", "Dokki" } },
            { "Qalyubia", new List<string> { "Benha", "Tukh", "Qalyoub", "El-Qanater El-Khairia", "Shubra El-Kheima", "El-Khanka", "Kafr Shokr", "El-Obour" } },
            { "New Valley", new List<string> { "Kharga", "Dakhla", "Farafra", "Paris", "Balat" } },
            { "Matrouh", new List<string> { "Marsa Matrouh", "El-Alamein", "Sidi Abdel Rahman", "El-Hamra" } },
            { "Minya", new List<string> { "Minya", "Mallawi", "Beni Mazar", "Samalut", "El-Edwa", "Deir Mawas", "El-Qusiya", "El-Minya" } },
            { "Fayoum", new List<string> { "Fayoum", "Etsa", "Ibshway", "Tamiya", "Youssef El-Seddik", "Sinuris", "Shakshouk" } },
            { "Beni Suef", new List<string> { "Beni Suef", "El-Fashn", "Nasser", "El-Wasta", "Ibsheway", "Sumosta", "Banha" } },
            { "Kafr El Sheikh", new List<string> { "Kafr El-Sheikh", "Desouq", "Qulubia", "Riyad", "Beyala", "Fuwah", "Haji", "Kafr El-Batikh" } },
            { "Port Said", new List<string> { "El-Gamil", "El-Manakh", "Abu Talat" } },
            { "Damietta", new List<string> { "Ras El-Bar", "Ezbet El-Borg", "Kafr Saad" } },
            { "Menoufia", new List<string> { "Shibin El-Kom", "Menouf", "Ashmoun", "Tala", "Berket El-Saba", "Sadat City", "Sers El-Lyan" } },
            { "Qena", new List<string> { "Qena", "Deshna", "Nag Hammadi", "Qous" } },
            { "Sohag", new List<string> { "Sohag", "Tahta", "El-Balyana", "Akhmim", "Girga", "Dar El-Salam" } },
            { "South Sinai", new List<string> { "Sharm El-Sheikh", "Dahab", "Nuweiba", "Taba", "Tor Sinai" } },
            { "North Sinai", new List<string> { "Arish", "Sheikh Zuweid", "Rafah", "Nakhl", "Bir El-Abd" } }
        };

            var cities = citiesByGovernate.ContainsKey(governorateId) ? citiesByGovernate[governorateId] : new List<string>();

            return Json(cities.Select(city => new { cityID = city, cityName = city }));
        }


        [HttpPost]
        public async Task<IActionResult> ToggleActivation(int technicianId, bool isActive)
        {
            var technician = await _DBcontext.Technicians.FindAsync(technicianId);
            if (technician == null)
            {
                return NotFound(new { success = false, message = "Technician not found" });
            }

            technician.IsActive = isActive;
            _DBcontext.Technicians.Update(technician);
            await _DBcontext.SaveChangesAsync();

            return Json(new { success = true });
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
                var result = await _technicianService.ImportTechniciansFromExcelAsync(stream);

                if (result)
                {
                    TempData["SuccessMessage"] = "technician imported successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to import technician. Please check the file format.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("GetAllTechnicians");
        }




        [HttpPost]
        public async Task<IActionResult> AssignCustomer(int technicianId, int customerId)
        {
            try
            {
                await _technicianService.AssignCustomerAsync(technicianId, customerId);
                return Json(new { success = true, message = "Customer assigned successfully." });
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return Json(new { success = false, message = ex.Message });
            }
        }


        // Remove Customer
        [HttpPost]
        public async Task<IActionResult> RemoveCustomer(int technicianId, string customerName)
        {
            await _technicianService.RemoveCustomerByNameAsync(technicianId, customerName);
            return Json(new { success = true, message = "Customer removed successfully." });
        }




        [HttpPost]
        public async Task<IActionResult> AssignUser(int technicianId, string userId)
        {
            try
            {
                await _technicianService.AssignRepresentativeAsync(technicianId, userId);
                return Json(new { success = true, message = "User assigned successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return Json(new { success = false, message = ex.Message });
            }
        }


        // Remove User
        [HttpPost]
        public async Task<IActionResult> RemoveUser(int technicianId, string userId)
        {
            // Remove the representative (assuming you have the RemoveRepresentativeAsync method)
            await _technicianService.RemoveRepresentativeAsync(technicianId, userId);
            return Json(new { success = true, message = "User removed successfully." });
        }


    }
}

