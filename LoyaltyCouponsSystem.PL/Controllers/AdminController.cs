using LoyaltyCouponsSystem.BLL.Service.Abstraction;
using LoyaltyCouponsSystem.BLL.ViewModel.Admin;
using LoyaltyCouponsSystem.DAL.DB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using LoyaltyCouponsSystem.BLL.Service.Implementation;
using LoyaltyCouponsSystem.BLL.ViewModel.Customer;

namespace LoyaltyCouponsSystem.PL.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IAdminService _adminService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext dbContext, IAdminService adminService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _adminService = adminService;
        }



        public async Task<IActionResult> ManageUsers()
        {
            var users = _userManager.Users.ToList();
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            var confirmedUsers = new List<AdminUserViewModel>();
            var unconfirmedUsers = new List<AdminUserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userModel = new AdminUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    NationalID = user.NationalID,
                    PhoneNumber = user.PhoneNumber,
                    OptionalPhoneNumber = user.OptionalPhoneNumber,
                    Governorate = user.Governorate,
                    City = user.City,
                    Role = roles.FirstOrDefault(),
                    EmailConfirmed = user.EmailConfirmed,
                    IsActive = user.IsActive, // Assuming IsActive is part of the ApplicationUser class
                    CreatedDate = user.CreatedDate // Ensure this is assigned

                };

                if (user.EmailConfirmed == true)
                    confirmedUsers.Add(userModel);
                else
                    unconfirmedUsers.Add(userModel);
            }

            var model = new ManageUsersViewModel
            {
                ConfirmedUsers = confirmedUsers,
                UnconfirmedUsers = unconfirmedUsers,
                AllRoles = allRoles
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ToggleActivation(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = !user.IsActive; // Toggle the IsActive property
                await _userManager.UpdateAsync(user);

                // Optionally, add logging or other actions here
            }

            return RedirectToAction("ManageUsers");
        }


        // Edit user
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _adminService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            user.EmailConfirmed = true;
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(AdminUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminService.UpdateUserAsync(model);
                if (result) return RedirectToAction("ManageUsers");
            }

            return View(model);
        }


        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _adminService.GetUserByIdForResetAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminService.ResetPasswordAsync(model);
                if (result) return RedirectToAction("ManageUsers");
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateUser(string id)
        {
            var user = await _adminService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            user.EmailConfirmed = true;


            var model = new AdminUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                OptionalPhoneNumber = user.OptionalPhoneNumber,
                NationalID = user.NationalID,
                Governorate = user.Governorate,
                City = user.City,
           };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(AdminUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminService.UpdateAsync(model);
                if (result) return RedirectToAction("ManageUsers");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID cannot be null or empty.");
            }

            var result = await _adminService.DeleteUserAsync(id);
            if (result)
            {
                return RedirectToAction("ManageUsers");
            }
            else
            {
                return NotFound();  // Return 404 if the user is not found or deletion fails
            }
        }
        [HttpGet]
        public async Task<IActionResult> AssignRoleForm()
        {
            var users = await _adminService.GetAllUsersAsync();
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            foreach (var user in users)
            {
                user.Roles = roles;
                user.SelectedRole = user.Role;
            }

            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            // Check if the user has the permission to assign roles
            

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("ManageUsers");
            }

            // Proceed with role assignment logic
            var result = await _adminService.AssignRoleToUserAsync(userId, roleName);
            if (result)
            {
                TempData["SuccessMessage"] = $"Role '{roleName}' has been assigned to user.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to assign the role.";
            }

            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmAccount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("ManageUsers");
            }

            user.EmailConfirmed = true;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User's account has been confirmed.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to confirm the account.";
            }

            return RedirectToAction("ManageUsers");
        }


        // Permmission 

        [HttpGet]
        public async Task<IActionResult> GetAssignedPermissions(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Fetch user's assigned permission IDs
            var assignedPermissionIds = await dbContext.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => up.PermissionId)  // Fetch PermissionId instead of PermissionName
                .ToListAsync();

            return Json(assignedPermissionIds);
        }

        [HttpPost]
        public async Task<IActionResult> AssignPermissionsToRole([FromBody] AssignPermissionsToRoleViewModel model)
         {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data received.");
            }

            await _adminService.AssignPermissionsToRoleAsync(model.RoleName, model.SelectedPermissions);
            return Ok(new { message = "Permissions assigned successfully!" });
        }
        [HttpPost]
        public async Task<IActionResult> AssignPermissionsToUser([FromBody] AssignPermissionsToUserViewModel model)
         {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data received.");
            }

            await _adminService.AssignPermissionsToUserAsync(model.UserId, model.SelectedPermissions);
            return Ok(new { message = "Permissions assigned successfully!" });
        }

        public async Task<IActionResult> GetUsersWithRolePermissions(string roleName)
        {
            // Fetch all users with permissions for the specified role
            var usersWithPermissions = await _adminService.GetUsersWithPermissionsAsync(roleName);

            // Do something with this data (e.g., log, process, etc.)
            foreach (var user in usersWithPermissions)
            {
                Console.WriteLine($"User: {user.UserName}, Permissions: {string.Join(", ", user.Permissions)}");
            }

            return Ok(); // Or any appropriate action result
        }
        public class PermissionRequirement : IAuthorizationRequirement
        {
            public string Permission { get; }

            public PermissionRequirement(string permission)
            {
                Permission = permission;
            }
        }

        public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ApplicationDbContext _dbContext;

            public PermissionHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
            {
                _userManager = userManager;
                _dbContext = dbContext;
            }

            protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
            {
                var user = await _userManager.GetUserAsync(context.User);
                if (user == null) return;

                // Get the roles assigned to the user
                var roles = await _userManager.GetRolesAsync(user);

                // Check if any of the user's roles have the required permission
                var hasPermission = await _dbContext.RolePermissions
                    .AnyAsync(rp => roles.Contains(rp.Role.Name) && rp.Permission.Name == requirement.Permission);

                if (hasPermission)
                {
                    context.Succeed(requirement);
                }
            }
        }


    }
}