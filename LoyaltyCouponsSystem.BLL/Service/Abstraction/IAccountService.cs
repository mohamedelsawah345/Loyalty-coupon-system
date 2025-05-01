using LoyaltyCouponsSystem.BLL.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.Service.Abstraction
{
    public interface IAccountService
    {
        Task<AdminUserViewModel> GetUserByUsernameAsync(string username);
    }
}
