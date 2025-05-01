using LoyaltyCouponsSystem.BLL.ViewModel.Distributor;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LoyaltyCouponsSystem.BLL.Service.Abstraction
{
    public interface IDistributorService
    {
        Task<bool> AddAsync(DistributorViewModel distributorViewModel);
        Task<bool> DeleteAsync(int id);
        Task<List<DistributorViewModel>> GetAllAsync();
        Task<DistributorViewModel> GetByIdAsync(int id);
        Task<bool> UpdateAsync(UpdateVM DistributorViewModel);
        Task<List<SelectListItem>> GetCustomersForDropdownAsync();
        Task<IEnumerable<SelectListItem>> GetGovernatesForDropdownAsync();
        Task<bool> ToggleActivationAsync(int distributorId);
        Task<bool> ImportDistributorsFromExcelAsync(Stream stream);
        Task<bool> RemoveCustomerFromDistributorByNameAsync(int distributorId, string customerName);   
        Task<bool> AddCustomerToDistributorAsync(int distributorId, int customerId);
    }
}
