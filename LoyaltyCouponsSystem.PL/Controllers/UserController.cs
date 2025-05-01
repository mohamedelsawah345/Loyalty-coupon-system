using LoyaltyCouponsSystem.BLL.Service.Abstraction;
using LoyaltyCouponsSystem.BLL.ViewModel.Account;
using LoyaltyCouponsSystem.DAL.DB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyCouponsSystem.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;

        public UserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IAccountService accountService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _context = context;
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            var roles = new List<string>
                {
               "مدير إدارة المعلومات والمتابعة",
                "مشرف إدارة المعلومات والمتابعة",
                "ممثل إدارة المعلومات والمتابعة",
                "مدير إدارة التسويق وخدمة العملاء",
                "مشرف إدارة التسويق وخدمة العملاء ",
                "مندوب إدارة التسويق وخدمة العملاء",
                "مدير إدارة الحسابات",
                "محاسب إدارة المخازن",
                "مراقب إدارة الحسابات",
                "مندوب تسليم إدارة المخازن",
                "مدير إدارة المبيعات",
                "منسق إدارة المبيعات",
                "مدير إدارة التشغيل",
                "منسق إدارة التشغيل",
                "مندوب تسويق وخدمة عملاء",
                
                };
            var addUserViewModel = new AddUserViewModel
            {
                AvailableRoles = roles
            };

            return View(addUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel addUserViewModel)
        {
            // Repopulate the roles list for the view in case of validation errors
            addUserViewModel.AvailableRoles = new List<string>
    {
                "مدير إدارة المعلومات والمتابعة",
                "مشرف إدارة المعلومات والمتابعة",
                "ممثل إدارة المعلومات والمتابعة",
                "مدير إدارة التسويق وخدمة العملاء",
                "مشرف إدارة التسويق وخدمة العملاء ",
                "مندوب إدارة التسويق وخدمة العملاء",
                "مدير إدارة الحسابات",
                "محاسب إدارة المخازن",
                "مراقب إدارة الحسابات",
                "مندوب تسليم إدارة المخازن",
                "مدير إدارة المبيعات",
                "منسق إدارة المبيعات",
                "مدير إدارة التشغيل",
                "منسق إدارة التشغيل",
                "مندوب تسويق وخدمة عملاء",
                

        };

            if (ModelState.IsValid)
            {
                // Log the selected role for debugging purposes
                Console.WriteLine($"Selected Role: {addUserViewModel.Role}");

                // Validate if NationalID already exists
                var existingUserByNationalID = await userManager.Users.FirstOrDefaultAsync(u => u.NationalID == addUserViewModel.NationalID);
                if (existingUserByNationalID != null)
                {
                    ModelState.AddModelError("NationalID", "The National ID is already in use.");
                }

                // Validate if PhoneNumber already exists
                var existingUserByPhoneNumber = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == addUserViewModel.PhoneNumber);
                if (existingUserByPhoneNumber != null)
                {
                    ModelState.AddModelError("PhoneNumber", "The phone number is already in use.");
                }

                // Validate OptionalPhoneNumber if provided
                if (!string.IsNullOrEmpty(addUserViewModel.OptionalPhoneNumber))
                {
                    var existingUserByOptionalPhone = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == addUserViewModel.OptionalPhoneNumber);
                    if (existingUserByOptionalPhone != null)
                    {
                        ModelState.AddModelError("OptionalPhoneNumber", "The optional phone number is already in use.");
                    }
                }

                // Ensure a role is selected
                if (string.IsNullOrEmpty(addUserViewModel.Role))
                {
                    ModelState.AddModelError("Role", "Please select a role.");
                }

                if (ModelState.IsValid) // Proceed only if there are no validation errors
                {
                    var user = new ApplicationUser
                    {
                        FullName = addUserViewModel.Name,
                        PhoneNumber = addUserViewModel.PhoneNumber,
                        Governorate = addUserViewModel.Governorate,
                        City = addUserViewModel.City,
                        NationalID = addUserViewModel.NationalID,
                        UserName = addUserViewModel.Name,
                        OptionalPhoneNumber = addUserViewModel.OptionalPhoneNumber,
                        Role = addUserViewModel.Role,
                        EmailConfirmed = true,
                    };

                    var result = await userManager.CreateAsync(user, addUserViewModel.Password);

                    if (result.Succeeded)
                    {
                        // Assign the selected role
                        var roleResult = await userManager.AddToRoleAsync(user, addUserViewModel.Role);
                        if (roleResult.Succeeded)
                        {
                            return RedirectToAction("ManageUsers", "Admin");  // Redirect to ManageUsers after successful registration
                        }
                        else
                        {
                            foreach (var error in roleResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }

            // Return to the same view with validation errors
            return View(addUserViewModel);
        }

    }
}
