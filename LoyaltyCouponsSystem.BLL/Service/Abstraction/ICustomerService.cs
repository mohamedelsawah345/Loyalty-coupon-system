using LoyaltyCouponsSystem.BLL.ViewModel.Customer;
using LoyaltyCouponsSystem.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.Service.Abstraction
{
    public interface ICustomerService
    {
        Task<bool> AddAsync(CustomerViewModel customerViewModel);
        Task<bool> DeleteAsync(int id);
        Task<List<CustomerViewModel>> GetAllAsync();
        Task<CustomerViewModel> GetByIdAsync(int id);
        Task<bool> UpdateAsync(UpdateCustomerViewModel updateCustomerViewModel);
        Task<bool> ToggleActivationAsync(int customerId, bool isActive);
        Task<bool> ImportCustomersFromExcelAsync(Stream stream);
        Task<bool> IsCodeUniqueAsync(string code);
    }
}
