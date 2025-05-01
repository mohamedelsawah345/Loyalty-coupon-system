
 using LoyaltyCouponsSystem.BLL.Service.Abstraction;
using LoyaltyCouponsSystem.BLL.ViewModel.Admin;
using LoyaltyCouponsSystem.DAL.DB;
 using LoyaltyCouponsSystem.DAL.Entity;
 using LoyaltyCouponsSystem.DAL.Entity.Permission;
 using Microsoft.AspNetCore.Identity;
 using Microsoft.EntityFrameworkCore;

namespace LoyaltyCouponsSystem.BLL.Service.Implementation
    {
        public class AdminService : IAdminService
        {
            private readonly ApplicationDbContext dbContext;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public AdminService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                this.dbContext = dbContext;
                _userManager = userManager;
                _roleManager = roleManager;
            }
            public async Task<IEnumerable<Representative>> GetAllRepresentativesAsync()
            {
                return await dbContext.Representatives
                    .Include(r => r.ApplicationUser) // Include related ApplicationUser data
                    .Include(r => r.Coupons) // Include related Coupons if necessary
                    .ToListAsync();
            }


            public async Task<IEnumerable<AdminUserViewModel>> GetAllUsersAsync()
            {
                var users = await dbContext.Users
            .Select(user => new AdminUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed,
                Role = user.Role,
                IsDeleted = user.IsDeleted
            }).ToListAsync();

                return users;
            }

            public async Task<AdminUserViewModel> GetUserByIdAsync(string userId)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return null;

                var roles = await _userManager.GetRolesAsync(user);

                return new AdminUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    OptionalPhoneNumber = user.OptionalPhoneNumber,
                    NationalID = user.NationalID,
                    Governorate = user.Governorate,
                    City = user.City,
                    Role = roles.FirstOrDefault(),
                };
            }
            public async Task<bool> UpdateUserAsync(AdminUserViewModel model)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null) return false;

                user.UserName = model.UserName;
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                var addRoleResult = await _userManager.AddToRoleAsync(user, model.Role);

                if (!addRoleResult.Succeeded)
                {
                    var errors = addRoleResult.Errors.Select(e => e.Description);

                }

                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            } 
            public async Task<bool> UpdateAsync(AdminUserViewModel model)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null) return false;

                user.UserName = model.UserName;
                user.NationalID = model.NationalID;
                user.PhoneNumber = model.PhoneNumber;
                user.OptionalPhoneNumber = model.OptionalPhoneNumber;
                user.Governorate = model.Governorate;
                user.City = model.City;

                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }

        public async Task<ResetPasswordViewModel> GetUserByIdForResetAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new ResetPasswordViewModel
            {
                Id = user.Id,
                
            };
        }
        public async Task<bool> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
            return result.Succeeded;
        }

            public async Task<bool> DeleteUserAsync(string userId)
            {
                try
                {
                    var user = await dbContext.Users
                        .FirstOrDefaultAsync(u => u.Id == userId);

                    if (user == null) return false;

                    // Set IsDeleted to true in ApplicationUser
                    user.IsDeleted = true;

                    dbContext.Users.Update(user);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting user: {ex.Message}");
                    return false;
                }
            }


        public async Task<bool> AssignRoleToUserAsync(string userId, string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                return false;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            // Remove all existing roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return false;
            }

            // Add the new role
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }



        public async Task<bool> UpdateUserRoleName(string userId, string roleName)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;

                user.Role = roleName;
                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();

                return true;
            }


            // Permissions 

            public async Task AssignPermissionsToRoleAsync(string roleName, List<string> permissionNames)
            {
                // Get the role
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null) return;

                // Get the list of permissions based on their names
                var permissions = await dbContext.Permissions
                    .Where(p => permissionNames.Contains(p.Name))
                    .ToListAsync();

                // Get all existing permissions for this role
                var existingRolePermissions = await dbContext.RolePermissions
        .Where(rp => rp.RoleId == role.Id)
        .Include(rp => rp.Permission)
        .ToListAsync();

                // Remove permissions that are not part of the new list
                var permissionsToRemove = existingRolePermissions
                    .Where(rp => !permissionNames.Contains(rp.Permission.Name))
                    .ToList();

                dbContext.RolePermissions.RemoveRange(permissionsToRemove);

                // Add new permissions that are not already assigned to the role
                foreach (var permission in permissions)
                {
                    var existingPermission = existingRolePermissions
                        .FirstOrDefault(rp => rp.PermissionId == permission.Id);

                    if (existingPermission == null)
                    {
                        // If the permission is not already assigned to the role, add it
                        var rolePermission = new RolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = permission.Id
                        };
                        dbContext.RolePermissions.Add(rolePermission);
                    }
                }

                // Save changes to the database
                await dbContext.SaveChangesAsync();
            }

            public async Task AssignPermissionsToUserAsync(string userId, List<string> permissionNames)
            {
                // Get the user by userId
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return;

                // Get the list of permissions based on their names
                var permissions = await dbContext.Permissions
                    .Where(p => permissionNames.Contains(p.Name))
                    .ToListAsync();

                // Get all existing permissions for this user
                var existingUserPermissions = await dbContext.UserPermissions
        .Where(up => up.UserId == user.Id)
        .Include(up => up.Permission) // Ensure Permission is loaded
        .ToListAsync();

                // Remove permissions that are not part of the new list
                var permissionsToRemove = existingUserPermissions
                    .Where(up => !permissionNames.Contains(up.Permission.Name))
                    .ToList();

                dbContext.UserPermissions.RemoveRange(permissionsToRemove);

                // Add new permissions that are not already assigned to the user
                foreach (var permission in permissions)
                {
                    var existingPermission = existingUserPermissions
                        .FirstOrDefault(up => up.PermissionId == permission.Id);

                    if (existingPermission == null)
                    {
                        // If the permission is not already assigned to the user, add it
                        var userPermission = new UserPermission
                        {
                            UserId = user.Id,
                            PermissionId = permission.Id
                        };
                        dbContext.UserPermissions.Add(userPermission);
                    }
                }

                // Save changes to the database
                await dbContext.SaveChangesAsync();
            }
            public async Task<List<string>> GetPermissionsForRoleAsync(string roleName)
            {
                // Find the role by name
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    return new List<string>(); // No permissions if role does not exist
                }

                // Assuming you have a way to get permissions for this role, e.g., through claims
                var permissions = await _roleManager.GetClaimsAsync(role);

                // Extract the permission names from claims (if you store them as claims)
                var permissionNames = permissions
                    .Where(c => c.Type == "Permission")
                    .Select(c => c.Value)
                    .ToList();

                return permissionNames;
            }
            public async Task<List<UserPermissionsModel>> GetUsersWithPermissionsAsync(string roleName)
            {
                // Get the role object by name
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null) return new List<UserPermissionsModel>();

                // Get all users with this role
                var users = await _userManager.GetUsersInRoleAsync(roleName);

                var userPermissionsList = new List<UserPermissionsModel>();

                foreach (var user in users)
                {
                    // Fetch the permissions for the user based on the role
                    var permissions = await GetPermissionsForRoleAsync(roleName);

                    userPermissionsList.Add(new UserPermissionsModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Permissions = permissions
                    });
                }

                return userPermissionsList;
            }
            public async Task<bool> UserHasPermissionAsync(string username, string permissionName)
            {
                // Get the user by their username
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return false; // User not found
                }

                // Get the list of permissions for this user
                var userPermissions = await dbContext.UserPermissions
                    .Where(up => up.UserId == user.Id)
                    .Include(up => up.Permission)
                    .ToListAsync();

                // Check if the user has the required permission
                return userPermissions.Any(up => up.Permission.Name == permissionName);
            }



        }
    }
   