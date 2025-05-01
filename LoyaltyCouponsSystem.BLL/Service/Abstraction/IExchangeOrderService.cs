using LoyaltyCouponsSystem.BLL.ViewModel.Customer;
using LoyaltyCouponsSystem.BLL.ViewModel.Distributor;
using LoyaltyCouponsSystem.BLL.ViewModel.Technician;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LoyaltyCouponsSystem.BLL.Service.Abstraction
{
    public interface IExchangeOrderService
    {
        Task<CustomerViewModel> GetCustomerDetailsAsync(string customerCodeOrName);
        Task<TechnicianViewModel> GetTechnicianDetailsAsync(string technicianCodeOrName);
        Task<AssignmentViewModel> GetAssignmentDetailsAsync();
        Task<DistributorViewModel> GetDistributorDetailsAsync(string technicianCodeOrName);
        Task<List<SelectListItem>> GetCustomersAsync();
        Task<List<SelectListItem>> GetDistributorsAsync();
        Task<List<SelectListItem>> GetGovernatesAsync();
        Task<List<SelectListItem>> GetCitiesByGovernateAsync(int governateId);
        Task AssignQRCodeAsync(string ExchangePermission, string selectedCustomerCode, string selectedDistributorCode, string selectedGovernate, string selectedCity, List<AssignmentViewModel> transactions);

        // New method for checking duplicate Exchange Permission Number
        Task<bool> IsExchangePermissionDuplicateAsync(string exchangePermission);
    }
}
