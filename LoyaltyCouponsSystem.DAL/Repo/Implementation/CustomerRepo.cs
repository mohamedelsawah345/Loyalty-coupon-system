using LoyaltyCouponsSystem.DAL.DB;
using LoyaltyCouponsSystem.DAL.Entity;
using LoyaltyCouponsSystem.DAL.Repo.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace LoyaltyCouponsSystem.DAL.Repo.Implementation
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly ApplicationDbContext _DBcontext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _DBcontext = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> IsUniquePhoneNumber(string phoneNumber)
        {
            return !await _DBcontext.Customers.AnyAsync(c => c.PhoneNumber == phoneNumber);
        }

        public async Task<bool> IsUniqueCode(string code)
        {
            code = code.Trim().ToLower();

            return !await _DBcontext.Customers
                .AnyAsync(c => c.Code.Trim().ToLower() == code);
        }





        // Add a customer
        public async Task<bool> AddAsync(Customer customer)
        {
            try
            {
                if (customer == null)
                    throw new ArgumentNullException(nameof(customer));
                var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);  // Access logged-in user
                customer.CreatedBy = currentUser?.UserName;
                customer.CreatedAt = DateTime.Now;
                await _DBcontext.Customers.AddAsync(customer);
                await _DBcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding customer: {ex.Message}");
                return false;
            }
        }

        // Soft delete a customer by ID (Code)
        public async Task<bool> DeleteAsync(int Id)
        {
            try
            {
                var customer = await _DBcontext.Customers.FirstOrDefaultAsync(c => c.CustomerID == Id);
                if (customer == null)
                    return false;
             
                _DBcontext.Customers.Remove(customer);
                await _DBcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting customer: {ex.Message}");
                return false;
            }
        }

        // Get all customers
        public async Task<List<Customer>> GetAllAsync()
        {
            try
            {
                // Add filter if soft-delete is implemented
                return await _DBcontext.Customers
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching customers: {ex.Message}");
                return new List<Customer>();
            }
        }

        // Get customer by Code
        public async Task<Customer> GetByIdAsync(int Id)
        {
            try
            {
                return await _DBcontext.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CustomerID == Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching customer by Code: {ex.Message}");
                return null;
            }
        }
        public async Task<List<int>> GetCustomerIdsByCodesAsync(List<string> customerCodes)
        {
            if (customerCodes == null || !customerCodes.Any())
            {
                return new List<int>(); // Return empty list if no codes are provided
            }

            return await _DBcontext.Customers
                .Where(c => customerCodes.Contains(c.Code))
                .Select(c => c.CustomerID)
                .ToListAsync();
        }

        public async Task<List<Customer>> GetCustomersByNamesAsync(List<string> customerNames)
        {
            return await _DBcontext.Customers
                .Where(c => customerNames.Contains(c.Name))
                .ToListAsync();
        }
        public async Task<List<Customer>> GetCustomersByIdsAsync(List<int> customerIds)
        {
            if (customerIds == null || customerIds.Count == 0)
            {
                return new List<Customer>(); // Return an empty list if no IDs are provided
            }

            // Fetch the customers from the database using the provided IDs
            return await _DBcontext.Customers
                .Where(c => customerIds.Contains(c.CustomerID))
                .ToListAsync();
        }     
        public async Task<bool> UpdateAsync(Customer customer)
        {
            try
            {
                if (customer == null)
                    throw new ArgumentNullException(nameof(customer));
                var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);  // Access logged-in user
                customer.UpdatedBy = currentUser?.UserName;
                customer.UpdatedAt = DateTime.Now;
                var existingCustomer = await _DBcontext.Customers
                    .FirstOrDefaultAsync(c => c.CustomerID == customer.CustomerID); // Fetch by ID

                if (existingCustomer == null)
                    return false;

                // Update properties
                existingCustomer.Name = customer.Name;
                existingCustomer.Code = customer.Code;
                existingCustomer.Governate = customer.Governate;
                existingCustomer.City = customer.City;
                existingCustomer.PhoneNumber = customer.PhoneNumber;
                existingCustomer.UpdatedAt = DateTime.Now;
                existingCustomer.UpdatedBy = currentUser?.UserName;
                await _DBcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating customer: {ex.Message}");
                return false;
            }
        }


        // Check if a customer exists by ID (Code)  
        public async Task<bool> ExistsAsync(string code)
        {
            try
            {
                return await _DBcontext.Customers.AnyAsync(c => c.Code == code);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking customer existence: {ex.Message}");
                return false;
            }
        }

        public async Task<Customer> GetByNameAsync(string customerName)
        {
            return await _DBcontext.Customers
                .FirstOrDefaultAsync(c => c.Name.Equals(customerName, StringComparison.OrdinalIgnoreCase));
        }

    }
}
