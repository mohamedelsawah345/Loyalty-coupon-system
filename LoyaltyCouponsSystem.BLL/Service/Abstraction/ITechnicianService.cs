using LoyaltyCouponsSystem.BLL.ViewModel.Customer;
using LoyaltyCouponsSystem.BLL.ViewModel.Technician;
using LoyaltyCouponsSystem.DAL.DB;
using LoyaltyCouponsSystem.DAL.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LoyaltyCouponsSystem.BLL.Service.Abstraction
{
    public interface ITechnicianService
    {
        Task<bool> AddAsync(TechnicianViewModel technicianViewModel);
        Task<bool> DeleteAsync(int id);
        Task<List<TechnicianViewModel>> GetAllAsync();
        Task<UpdateTechnicianViewModel> GetByIdAsync(int id);
        Task<bool> UpdateAsync(UpdateTechnicianViewModel TechnicianViewModel);
        Task<List<SelectListItem>> GetCustomersForDropdownAsync();
        Task<List<SelectListItem>> GetUsersForDropdownAsync();
        Task<bool> ToggleActivationAsync(int distributorId);
        Task<bool> ImportTechniciansFromExcelAsync(Stream stream);
        Task AssignCustomerAsync(int technicianId, int customerId);
        Task RemoveCustomerByNameAsync(int technicianId, string customerName);
        Task<List<CustomerViewModel>> GetUnassignedActiveCustomersAsync(int technicianId);
        Task AssignRepresentativeAsync(int technicianId, string userId);
        Task RemoveRepresentativeAsync(int technicianId, string userId);
        Task<List<ApplicationUser>> GetActiveUnassignedRepresentativesAsync(int technicianId);
        Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName);
    }
}
