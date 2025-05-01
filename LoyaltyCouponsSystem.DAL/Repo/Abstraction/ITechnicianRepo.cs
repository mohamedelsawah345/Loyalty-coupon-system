using LoyaltyCouponsSystem.DAL.DB;
using LoyaltyCouponsSystem.DAL.Entity;

namespace LoyaltyCouponsSystem.DAL.Repo.Abstraction
{
    public interface ITechnicianRepo
    {
        Task<bool> IsUniqueCodeAsync(string code);
        Task<bool> IsUniquePhoneNumberAsync(string phoneNumber);
        Task<bool> AddAsync(Technician technician, List<string> customerCodes, List<string> userIds);
        Task<bool> DeleteAsync(int id);
        Task<List<Technician>> GetAllAsync();
        Task<Technician> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Technician technician, List<int> customerIds, List<string> userIds);
        Task<List<Customer>> GetCustomersForDropdownAsync();
        Task<List<ApplicationUser>> GetUsersForDropdownAsync();
        Task AssignCustomerAsync(int technicianId, int customerId);
        Task AssignUserAsync(int technicianId, string userId); 
        Task<List<ApplicationUser>> GetActiveUnassignedRepresentativesAsync(int technicianId);
        Task<List<Customer>> GetActiveUnassignedCustomersAsync(int technicianId);
        Task RemoveCustomerByNameAsync(int technicianId, string customerName);
        Task RemoveUserByNameAsync(int technicianId, string userName);
        Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName);
    }
}
