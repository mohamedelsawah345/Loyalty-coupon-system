using LoyaltyCouponsSystem.BLL.ViewModel.Admin;
using LoyaltyCouponsSystem.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.Service.Abstraction
{
    public interface IAdminService
    {
        Task<IEnumerable<AdminUserViewModel>> GetAllUsersAsync();
        Task<AdminUserViewModel> GetUserByIdAsync(string userId);
        Task<ResetPasswordViewModel> GetUserByIdForResetAsync(string userId);
        Task<bool> UpdateUserAsync(AdminUserViewModel model);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> AssignRoleToUserAsync(string userId, string roleName);
        Task<bool> UpdateUserRoleName(string userId, string roleName);
        Task AssignPermissionsToRoleAsync(string roleName, List<string> permissionNames);
        Task AssignPermissionsToUserAsync(string userId, List<string> permissionNames);
        Task<bool> UserHasPermissionAsync(string username, string permissionName);
        Task<List<UserPermissionsModel>> GetUsersWithPermissionsAsync(string roleName);
        Task<List<string>> GetPermissionsForRoleAsync(string roleName);
        Task<bool> UpdateAsync(AdminUserViewModel model);
        Task<bool> ResetPasswordAsync(ResetPasswordViewModel model);


    }
}