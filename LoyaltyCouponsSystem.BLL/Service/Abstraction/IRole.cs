using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.Service.Abstraction
{
    public interface IRole
    {
        Task<bool> AssignRoleAsync(string userId, string role);
        Task<string> GetUserRoleAsync(string userId);
    }
}
