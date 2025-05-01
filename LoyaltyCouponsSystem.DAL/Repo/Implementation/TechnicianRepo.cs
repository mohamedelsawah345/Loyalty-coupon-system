using LoyaltyCouponsSystem.DAL.DB;
using LoyaltyCouponsSystem.DAL.Entity;
using LoyaltyCouponsSystem.DAL.Repo.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyCouponsSystem.DAL.Repo.Implementation
{
    public class TechnicianRepo : ITechnicianRepo
    {
        private readonly ApplicationDbContext _DBcontext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TechnicianRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _DBcontext = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        // Check if Technician Code is Unique
        public async Task<bool> IsUniqueCodeAsync(string code)
        {
            return !await _DBcontext.Technicians.AnyAsync(t => t.Code == code);
        }

        // Check if PhoneNumber1 is Unique
        public async Task<bool> IsUniquePhoneNumberAsync(string phoneNumber)
        {
            return !await _DBcontext.Technicians.AnyAsync(t => t.PhoneNumber1 == phoneNumber);
        }

        // Add a New Technician
        public async Task<bool> AddAsync(Technician technician, List<string> customerCodes, List<string> userIds)
        {
            try
            {
                // Ensure the Code and Phone Number are unique
                if (!await IsUniqueCodeAsync(technician.Code))
                    throw new Exception("Code already exists.");
                if (!await IsUniquePhoneNumberAsync(technician.PhoneNumber1))
                    throw new Exception("Phone number already exists.");

                var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);  // Access logged-in user
                technician.CreatedBy = currentUser?.UserName;
                technician.CreatedAt = DateTime.Now;

                // Fetch customers by their codes using raw SQL query with specified collation
                var customers = new List<Customer>();
                if (customerCodes != null && customerCodes.Any())
                {
                    customers = await _DBcontext.Customers
                        .FromSqlRaw("SELECT * FROM Customers WHERE Code COLLATE SQL_Latin1_General_CP1_CI_AS IN ({0})", string.Join(",", customerCodes))
                        .ToListAsync();
                }

                // Map TechnicianCustomer relationships from customerCodes
                technician.TechnicianCustomers = customers.Select(customer => new TechnicianCustomer
                {
                    CustomerId = customer.CustomerID,  // Use the fetched Customer ID
                    TechnicianId = technician.TechnicianID
                }).ToList();

                // Create TechnicianUser relationships from userIds (filtering for the new roles)
                var users = new List<ApplicationUser>();
                if (userIds != null && userIds.Any())
                {
                    users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
                }

                var validUsers = new List<ApplicationUser>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("مندوب تسليم إدارة المخازن") || roles.Contains("مندوب إدارة التسويق وخدمة العملاء"))
                    {
                        validUsers.Add(user);
                    }
                }

                if (validUsers.Any())
                {
                    technician.TechnicianUsers = validUsers.Select(user => new TechnicianUser
                    {
                        UserId = user.Id,
                        TechnicianId = technician.TechnicianID
                    }).ToList();
                }

                // Add technician and relationships to the database
                await _DBcontext.Technicians.AddAsync(technician);
                await _DBcontext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding technician: {ex.Message}");
                return false;
            }
        }




        // Delete Technician
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var technician = await _DBcontext.Technicians
                    .Include(t => t.TechnicianCustomers)
                    .Include(t => t.TechnicianUsers)
                    .FirstOrDefaultAsync(t => t.TechnicianID == id);

                if (technician != null)
                {
                    _DBcontext.TechnicianCustomers.RemoveRange(technician.TechnicianCustomers);
                    _DBcontext.TechnicianUsers.RemoveRange(technician.TechnicianUsers);
                    _DBcontext.Technicians.Remove(technician);
                    await _DBcontext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting technician: {ex.Message}");
                return false;
            }
        }

        // Get All Technicians
        public async Task<List<Technician>> GetAllAsync()
        {
            try
            {
                return await _DBcontext.Technicians
                    .Include(t => t.TechnicianCustomers).ThenInclude(tc => tc.Customer)
                    .Include(t => t.TechnicianUsers).ThenInclude(tu => tu.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching technicians: {ex.Message}");
                return new List<Technician>();
            }
        }

        // Get Technician by ID
        public async Task<Technician> GetByIdAsync(int id)
        {
            try
            {
                return await _DBcontext.Technicians
                    .Include(t => t.TechnicianCustomers).ThenInclude(tc => tc.Customer)
                    .Include(t => t.TechnicianUsers).ThenInclude(tu => tu.User)
                    .FirstOrDefaultAsync(t => t.TechnicianID == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching technician by ID: {ex.Message}");
                return null;
            }
        }

        // Update Technician
        public async Task<bool> UpdateAsync(Technician technician, List<int> customerIds, List<string> userIds)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);  // Access logged-in user
                technician.UpdatedBy = currentUser?.UserName;
                technician.UpdatedAt = DateTime.Now;

                var existingTechnician = await _DBcontext.Technicians
                    .Include(t => t.TechnicianCustomers)
                    .Include(t => t.TechnicianUsers)
                    .FirstOrDefaultAsync(t => t.TechnicianID == technician.TechnicianID);

                if (existingTechnician == null)
                {
                    return false;
                }

                // Update basic properties
                existingTechnician.Name = technician.Name;
                existingTechnician.NickName = technician.NickName;
                existingTechnician.NationalID = technician.NationalID;
                existingTechnician.PhoneNumber1 = technician.PhoneNumber1;
                existingTechnician.PhoneNumber2 = technician.PhoneNumber2;
                existingTechnician.PhoneNumber3 = technician.PhoneNumber3;
                existingTechnician.Governate = technician.Governate;
                existingTechnician.City = technician.City;
                existingTechnician.Code = technician.Code;

                // Update Customers
                _DBcontext.TechnicianCustomers.RemoveRange(existingTechnician.TechnicianCustomers);
                existingTechnician.TechnicianCustomers = customerIds.Select(id => new TechnicianCustomer { CustomerId = id }).ToList();

                // Update Users
                _DBcontext.TechnicianUsers.RemoveRange(existingTechnician.TechnicianUsers);
                existingTechnician.TechnicianUsers = userIds.Select(id => new TechnicianUser { UserId = id }).ToList();

                await _DBcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating technician: {ex.Message}");
                return false;
            }
        }

        // Get Customers for Dropdown
        public async Task<List<Customer>> GetCustomersForDropdownAsync()
        {
            try
            {
                return await _DBcontext.Customers
                    .Where(c => c.IsActive)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching customers for dropdown: {ex.Message}");
                return new List<Customer>();
            }
        }

        // Get Users for Dropdown
        public async Task<List<ApplicationUser>> GetUsersForDropdownAsync()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var representatives = new List<ApplicationUser>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (user.IsActive && (roles.Contains("مندوب تسليم إدارة المخازن") || roles.Contains("مندوب إدارة التسويق وخدمة العملاء")))
                    {
                        representatives.Add(user);
                    }
                }

                return representatives;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users for dropdown: {ex.Message}");
                return new List<ApplicationUser>();
            }
        }


        // Assign Customer to Technician
        public async Task AssignCustomerAsync(int technicianId, int customerId)
        {
            var technician = await _DBcontext.Technicians
                .Include(t => t.TechnicianCustomers)
                .FirstOrDefaultAsync(t => t.TechnicianID == technicianId);

            if (technician != null && !technician.TechnicianCustomers.Any(tc => tc.CustomerId == customerId))
            {
                technician.TechnicianCustomers.Add(new TechnicianCustomer { CustomerId = customerId });
                await _DBcontext.SaveChangesAsync();
            }
        }

        // Assign User to Technician
        public async Task AssignUserAsync(int technicianId, string userId)
        {
            var technician = await _DBcontext.Technicians
                .Include(t => t.TechnicianUsers)
                .FirstOrDefaultAsync(t => t.TechnicianID == technicianId);

            if (technician != null && !technician.TechnicianUsers.Any(tu => tu.UserId == userId))
            {
                technician.TechnicianUsers.Add(new TechnicianUser { UserId = userId });
                await _DBcontext.SaveChangesAsync();
            }
        }

        // Get Active Unassigned Customers
        public async Task<List<Customer>> GetActiveUnassignedCustomersAsync(int technicianId)
        {
            // Fetch all active customers who are not assigned to the given technician
            var allCustomers = await _DBcontext.Customers
                .Where(c => c.IsActive)
                .ToListAsync();

            var unassignedCustomers = allCustomers
                .Where(c => c.TechnicianCustomers == null || !c.TechnicianCustomers.Any(tc => tc.TechnicianId == technicianId))
                .ToList();

            return unassignedCustomers;
        }

        public async Task<List<ApplicationUser>> GetActiveUnassignedRepresentativesAsync(int technicianId)
        {
            try
            {
                var allUsers = await _userManager.Users.ToListAsync();
                var representatives = new List<ApplicationUser>();

                foreach (var user in allUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (user.IsActive && (roles.Contains("مندوب تسليم إدارة المخازن") || roles.Contains("مندوب إدارة التسويق وخدمة العملاء")))
                    {
                        representatives.Add(user);
                    }
                }

                var unassignedRepresentatives = representatives
                    .Where(u => u.TechnicianUsers == null || !u.TechnicianUsers.Any(tu => tu.TechnicianId == technicianId))
                    .ToList();

                return unassignedRepresentatives;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching active unassigned representatives: {ex.Message}");
                return new List<ApplicationUser>();
            }
        }




        public async Task RemoveCustomerByNameAsync(int technicianId, string customerName)
        {
            // Find the customer by name
            var customer = await _DBcontext.Customers
                .FirstOrDefaultAsync(c => c.Name == customerName);

            if (customer == null)
            {
                return; // Customer not found
            }

            // Find the relationship between the technician and the customer
            var technicianCustomer = await _DBcontext.TechnicianCustomers
                .FirstOrDefaultAsync(tc => tc.TechnicianId == technicianId && tc.CustomerId == customer.CustomerID);

            if (technicianCustomer != null)
            {
                // Remove the relationship
                _DBcontext.TechnicianCustomers.Remove(technicianCustomer);
                await _DBcontext.SaveChangesAsync();
            }
        }


        public async Task RemoveUserByNameAsync(int technicianId, string userName)
        {
            // Find the user by their username
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user != null)
            {
                // Find the technician-user association using the technicianId and userId
                var technicianUser = await _DBcontext.TechnicianUsers
                    .FirstOrDefaultAsync(tu => tu.TechnicianId == technicianId && tu.UserId == user.Id);

                if (technicianUser != null)
                {
                    // Remove the association between the technician and the user
                    _DBcontext.TechnicianUsers.Remove(technicianUser);
                    await _DBcontext.SaveChangesAsync();
                }
            }
        }

        public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName)
        {
            // Ensure the role exists
            var role = await _userManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return null; // Role not found
            }

            // Get all users assigned to this role
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

            return usersInRole.ToList();
        }

    }
}
