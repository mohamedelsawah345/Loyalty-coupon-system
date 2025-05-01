using global::LoyaltyCouponsSystem.DAL.DB;
using global::LoyaltyCouponsSystem.DAL.Entity;
using LoyaltyCouponsSystem.DAL.Repo.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LoyaltyCouponsSystem.DAL.Repo.Implementation
{
    public class ExchangeOrderRepo : IExchangeOrderRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExchangeOrderRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<Customer> GetCustomerByCodeOrNameAsync(string customerCodeOrName)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Code == customerCodeOrName || c.Name == customerCodeOrName);
        }

        public async Task<Technician> GetTechnicianByCodeOrNameAsync(string technicianCodeOrName)
        {
            return await _context.Technicians
                .FirstOrDefaultAsync(t => t.Code == technicianCodeOrName || t.Name == technicianCodeOrName);
        }
        public async Task<Distributor> GetDistributorByCodeOrNameAsync(string technicianCodeOrName)
        {
            return await _context.Distributors
                .FirstOrDefaultAsync(at => at.Code == technicianCodeOrName || at.Name == technicianCodeOrName);
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers
                         .Where(c => c.IsActive) 
                         .GroupBy(c => new {
                             Name = c.Name.Trim().ToLower(),
                             Code = c.Code.Trim().ToLower() 
                         })
                         .Select(g => g.First()) 
                         .ToListAsync();
        }

        public async Task<List<Distributor>> GetAllDistributorsAsync()
        {
            return await _context.Distributors
                         .Where(d => d.IsActive) 
                         .GroupBy(d => new {
                             Name = d.Name.Trim().ToLower(),
                             Code = d.Code.Trim().ToLower() 
                         })
                         .Select(g => g.First()) 
                         .ToListAsync();
        }
        public async Task<List<SelectListItem>> GetGovernatesAsync()
        {
            return await _context.Governorates
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                })
                .ToListAsync();
        }

        // Get cities based on selected governorate
        public async Task<List<SelectListItem>> GetCitiesByGovernateAsync(string governateId)
        {
            return await _context.Areas
                .Where(c => c.GovernateId.ToString() == governateId)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();
        }


        public async Task<List<Technician>> GetAllTechniciansAsync()
        {
            return await _context.Technicians.ToListAsync();
        }
        public async Task AddTransactionAsync(Transaction transaction)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);  // Access logged-in user
            transaction.CreatedBy = currentUser?.UserName;
            await _context.Transactions.AddAsync(transaction);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> TransactionExistsAsync(string exchangePermission, long sequenceNumber)
        {
            return await _context.Transactions.AnyAsync(t =>
                t.ExchangePermission == exchangePermission &&
                t.SequenceNumber == sequenceNumber);
        }

        // New method to check for duplicate Exchange Permission Number
        public async Task<bool> ExchangePermissionExistsAsync(string exchangePermission)
        {
            return await _context.Transactions.AnyAsync(t => t.ExchangePermission == exchangePermission);
        }
        public async Task<Area> GetAreaByIdAsync(int id)
        {
            return await _context.Areas
                                 .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<Governorate> GetGovernorateByIdAsync(int id)
        {
            return await _context.Governorates
                                 .FirstOrDefaultAsync(g => g.Id == id);
        }


    }
}
