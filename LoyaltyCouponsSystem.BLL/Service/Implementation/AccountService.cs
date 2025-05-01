using LoyaltyCouponsSystem.BLL.Service.Abstraction;
using LoyaltyCouponsSystem.BLL.ViewModel.Admin;
using LoyaltyCouponsSystem.DAL.DB;
using Microsoft.AspNetCore.Identity;

namespace LoyaltyCouponsSystem.BLL.Service.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<AdminUserViewModel> GetUserByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new AdminUserViewModel
            {
                FullName = user.FullName
            };
        }
        
    }
}
