using LoyaltyCouponsSystem.DAL.Entity;

namespace LoyaltyCouponsSystem.DAL.Repo.Abstraction
{
    public interface IDistributorRepo
    {
        Task<bool> AddAsync(Distributor distributor);
        Task<bool> DeleteAsync(int id);
        Task<List<Distributor>> GetAllAsync();
        Task<Distributor> GetByIdAsync(int Id);
        Task<bool> UpdateAsync(Distributor distributor);
        Task<List<Customer>> GetCustomersForDropdownAsync();
        Task<bool> IsUniquePhoneNumberAsync(string phoneNumber);
        Task<bool> IsUniqueCodeAsync(string code);
        Task<Customer> GetCustomerByIdAsync(int customerId);
        Task SaveAsync();
        Task<Distributor> GetDistributorByIdAsync(int distributorId);

    }
}
