using LoyaltyCouponsSystem.DAL.Entity;

namespace LoyaltyCouponsSystem.DAL.Repo.Abstraction
{
    public interface ICustomerRepo
    {
        Task<bool> AddAsync(Customer customer);
        Task<bool> DeleteAsync(int Id);
        Task<List<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int Id);
        Task<bool> UpdateAsync(Customer customer);
        Task<List<Customer>> GetCustomersByIdsAsync(List<int> customerIds);
        Task<bool> IsUniqueCode(string code);
        Task<bool> IsUniquePhoneNumber(string phoneNumber);
        Task<List<int>> GetCustomerIdsByCodesAsync(List<string> customerCodes);
        Task<Customer> GetByNameAsync(string customerName);
        Task<List<Customer>> GetCustomersByNamesAsync(List<string> customerNames);
    }
}
