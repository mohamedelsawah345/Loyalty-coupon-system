namespace LoyaltyCouponsSystem.PL.Controllers
{
    using LoyaltyCouponsSystem.BLL.Service.Abstraction;
    using LoyaltyCouponsSystem.BLL.ViewModel.Account;
    using LoyaltyCouponsSystem.DAL.DB;
    
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    namespace UsersApp.Controllers
    {
        public class AccountController : Controller
        {
            private readonly SignInManager<ApplicationUser> signInManager;
            private readonly UserManager<ApplicationUser> userManager;
            private readonly ApplicationDbContext _context;
            private readonly IAccountService _accountService;

            public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IAccountService accountService)
            {
                this.signInManager = signInManager;
                this.userManager = userManager;
                _context = context;
                _accountService = accountService;
            }

            public IActionResult Login()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Login(LoginViewModel model)
            {
                if (ModelState.IsValid)
                {
                    var result = await signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        var userDetails = await _accountService.GetUserByUsernameAsync(model.Name);

                        // Serialize user details to TempData if necessary
                        TempData["UserDetails"] = JsonConvert.SerializeObject(userDetails, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

                        // Check if the user is a SuperAdmin
                        var user = await userManager.FindByNameAsync(model.Name);
                        var roles = await userManager.GetRolesAsync(user);

                        if (roles.Contains("SuperAdmin"))
                        {
                            // Redirect to ManageUsers for SuperAdmin
                            return RedirectToAction("ManageUsers", "Admin");
                        }

                        // Redirect to home if not SuperAdmin
                        return RedirectToAction("Home", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login credentials. Please try again!");
                        return View(model);
                    }
                }
                return View(model);
            }



            public IActionResult Register()
            {
                ViewBag.Roles = new List<SelectListItem>
                {
                    new SelectListItem { Value = "مندوب تسليم إدارة المخازن", Text = "مندوب تسليم إدارة المخازن" },
                    new SelectListItem { Value = "مندوب إدارة التسويق وخدمة العملاء", Text = "مندوب إدارة التسويق وخدمة العملاء" }
                };

                return View(new RegisterViewModel()); 
            }

            [HttpPost]
            public async Task<IActionResult> Register(RegisterViewModel model)
            {
                if (ModelState.IsValid)
                {
                    // Validate National ID uniqueness
                    var existingUserByNationalID = await userManager.Users.FirstOrDefaultAsync(u => u.NationalID == model.NationalID);
                    if (existingUserByNationalID != null)
                    {
                        ModelState.AddModelError("NationalID", "The National ID is already in use.");
                        return View(model);
                    }

                    // Validate Phone Number uniqueness
                    var existingUserByPhoneNumber = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
                    if (existingUserByPhoneNumber != null)
                    {
                        ModelState.AddModelError("PhoneNumber", "The phone number is already in use.");
                        return View(model);
                    }

                    // Validate Optional Phone Number
                    if (!string.IsNullOrEmpty(model.OptionalPhoneNumber))
                    {
                        var existingUserByOptionalPhone = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.OptionalPhoneNumber);
                        if (existingUserByOptionalPhone != null)
                        {
                            ModelState.AddModelError("OptionalPhoneNumber", "The optional phone number is already in use.");
                            return View(model);
                        }
                    }

                    if (string.IsNullOrEmpty(model.Role))
                    {
                        ModelState.AddModelError("Role", "Please select a role.");
                        return View(model);
                    }

                    var user = new ApplicationUser
                    {
                        FullName = model.Name,
                        PhoneNumber = model.PhoneNumber,
                        Governorate = model.Governorate,
                        City = model.City,
                        NationalID = model.NationalID,
                        Role = model.Role, // Assign selected role
                        UserName = model.Name,
                        OptionalPhoneNumber = model.OptionalPhoneNumber
                    };

                    var result = await userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        // Assign selected role instead of hardcoded "Representative"
                        await userManager.AddToRoleAsync(user, model.Role);
                        ViewBag.RegistrationSuccess = true;
                        return View("Register");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }

                ViewBag.RegistrationSuccess = false;
                return View(model);
            }




            [HttpGet]
            [Route("api/get-cities")]
            public IActionResult GetCities(string governorate)
            {
                var cities = new Dictionary<string, List<string>>
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

                if (cities.ContainsKey(governorate))
                {
                    return Json(cities[governorate]);
                }

                return Json(new List<string>()); // Return an empty list if no cities found
            }




            [HttpGet]
            [Route("api/check-phone-number")]
            public IActionResult CheckPhoneNumber(string phoneNumber)
            {
                // Check if the phone number already exists in the database
                var isUnique = !_context.Users.Any(u => u.PhoneNumber == phoneNumber);
                return Json(new { isUnique });
            }


            [HttpGet]
            public async Task<IActionResult> CheckNationalID(string nationalID)
            {
                var exists = await userManager.Users.AnyAsync(u => u.NationalID == nationalID);
                return Json(new { isUnique = !exists });
            }

            public IActionResult VerifyEmail()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByNameAsync(model.Name);

                    if (user == null)
                    {
                        ModelState.AddModelError("", "Email is not correct!");
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                    }
                }
                return View(model);
            }

            public IActionResult ChangePassword(string username)
            {
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToAction("VerifyEmail", "Account");
                }
                return View(new ChangePasswordViewModel { Name = username });
            }

            [HttpPost]
            public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByNameAsync(model.Name);
                    if (user != null)
                    {
                        var result = await userManager.RemovePasswordAsync(user);
                        if (result.Succeeded)
                        {
                            result = await userManager.AddPasswordAsync(user, model.NewPassword);
                            return RedirectToAction("Log    in", "Account");
                        }
                        else
                        {

                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }

                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email not found!");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong. try again.");
                    return View(model);
                }
            }
            public IActionResult AccessDenied()
            {
                return View();
            }

            public async Task<IActionResult> Logout()
            {
                await signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            public IActionResult Home()
            {
                return View();
            }
        }
    }
}
