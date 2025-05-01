using LoyaltyCouponsSystem.BLL.Service.Abstraction;
using LoyaltyCouponsSystem.BLL.ViewModel.Customer;
using LoyaltyCouponsSystem.BLL.ViewModel.Technician;
using LoyaltyCouponsSystem.DAL.DB;
using LoyaltyCouponsSystem.DAL.Entity;
using LoyaltyCouponsSystem.DAL.Repo.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace LoyaltyCouponsSystem.BLL.Service.Implementation
{
    public class TechnicianService : ITechnicianService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITechnicianRepo _technicianRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        public TechnicianService(ITechnicianRepo technicianRepo, ICustomerRepo customerRepo, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _technicianRepo = technicianRepo;
            _customerRepo = customerRepo;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> AddAsync(TechnicianViewModel technicianViewModel)
        {
            try
            {
                // Map TechnicianViewModel to Technician entity
                var technician = new Technician
                {
                    Code = technicianViewModel.Code,
                    Name = technicianViewModel.Name,
                    NickName = technicianViewModel.NickName,
                    NationalID = technicianViewModel.NationalID,
                    PhoneNumber1 = technicianViewModel.PhoneNumber1,
                    PhoneNumber2 = technicianViewModel.PhoneNumber2,
                    PhoneNumber3 = technicianViewModel.PhoneNumber3,
                    Governate = technicianViewModel.SelectedGovernate,
                    City = technicianViewModel.SelectedCity,
                    CreatedAt = DateTime.Now,
                    CreatedBy = (await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User))?.UserName,
                    IsActive = technicianViewModel.IsActive
                };

                // Pass customer IDs and user IDs to the repository for processing
                return await _technicianRepo.AddAsync(technician, technicianViewModel.SelectedCustomerId, technicianViewModel.SelectedUserCodes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding technician: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> DeleteAsync(int id)
        {
            if (id != 0)
            {
                return await _technicianRepo.DeleteAsync(id);
            }
            return false;
        }

        public async Task<List<TechnicianViewModel>> GetAllAsync()
        {
            var technicians = await _technicianRepo.GetAllAsync();

            // Get all active customers and users (you can decide to optimize this depending on your use case)
            var customers = await _customerRepo.GetAllAsync();
            var users = await _userManager.Users.ToListAsync();

            // Transform technicians into TechnicianViewModels and order by CreatedAt descending
            var technicianViewModels = technicians
                .OrderByDescending(t => t.CreatedAt) // Order by newest (CreatedAt)
                .Select(technician => new TechnicianViewModel
                {
                    TechnicianID = technician.TechnicianID,
                    Code = technician.Code,
                    Name = technician.Name,
                    NickName = technician.NickName,
                    NationalID = technician.NationalID,
                    PhoneNumber1 = technician.PhoneNumber1,
                    PhoneNumber2 = technician.PhoneNumber2,
                    PhoneNumber3 = technician.PhoneNumber3,
                    SelectedGovernate = technician.Governate,
                    SelectedCity = technician.City,
                    CreatedAt = technician.CreatedAt,
                    CreatedBy = technician.CreatedBy,
                    UpdatedBy = technician.UpdatedBy,
                    UpdatedAt = technician.UpdatedAt,
                    IsActive = technician.IsActive,
                    // Selecting associated customer and user names
                    SelectedCustomerNames = technician.TechnicianCustomers.Select(tc => tc.Customer.Name).ToList(),
                    SelectedUserNames = technician.TechnicianUsers.Select(tu => tu.User.UserName).ToList(),
                })
                .ToList();

            // Populate dropdowns if needed (e.g., active customers, active users)
            foreach (var technicianViewModel in technicianViewModels)
            {
                PopulateDropdowns(technicianViewModel);
            }

            return technicianViewModels;
        }

        public async Task<UpdateTechnicianViewModel> GetByIdAsync(int id)
        {
            if (id != 0)
            {
                // Fetch the technician by ID from the repository
                var technician = await _technicianRepo.GetByIdAsync(id);

                if (technician != null)
                {
                    // Create the UpdateTechnicianViewModel based on the technician data
                    var updateTechnicianViewModel = new UpdateTechnicianViewModel
                    {
                        TechnicianID = technician.TechnicianID,
                        Code = technician.Code,
                        Name = technician.Name,
                        NickName = technician.NickName,
                        NationalID = technician.NationalID,
                        PhoneNumber1 = technician.PhoneNumber1,
                        PhoneNumber2 = technician.PhoneNumber2,
                        PhoneNumber3 = technician.PhoneNumber3,
                        SelectedGovernate = technician.Governate,
                        SelectedCity = technician.City,
                        IsActive = technician.IsActive,
                        // Use the repository methods to fetch customers and users for the dropdowns
                        Customers = (await _technicianRepo.GetCustomersForDropdownAsync())
                                    .Select(c => new SelectListItem
                                    {
                                        Value = c.Code,
                                        Text = $"{c.Code} - {c.Name}"
                                    }).ToList(),
                        Users = (await _technicianRepo.GetUsersForDropdownAsync())
                                .Select(u => new SelectListItem
                                {
                                    Value = u.Id,
                                    Text = $"{u.UserName}"
                                }).ToList()
                    };

                    return updateTechnicianViewModel;
                }
            }
            return null;
        }

        public async Task<bool> UpdateAsync(UpdateTechnicianViewModel technicianViewModel)
        {
            if (technicianViewModel != null)
            {
                // Fetch the existing technician entity from the database
                var existingTechnician = await _technicianRepo.GetByIdAsync(technicianViewModel.TechnicianID);
                if (existingTechnician == null)
                {
                    return false; // Technician not found
                }

                // Update the basic fields
                existingTechnician.TechnicianID = technicianViewModel.TechnicianID;
                existingTechnician.Code = technicianViewModel.Code;
                existingTechnician.Name = technicianViewModel.Name;
                existingTechnician.NickName = technicianViewModel.NickName;
                existingTechnician.NationalID = technicianViewModel.NationalID;
                existingTechnician.PhoneNumber1 = technicianViewModel.PhoneNumber1;
                existingTechnician.PhoneNumber2 = technicianViewModel.PhoneNumber2;
                existingTechnician.PhoneNumber3 = technicianViewModel.PhoneNumber3;
                existingTechnician.Governate = technicianViewModel.SelectedGovernate;
                existingTechnician.City = technicianViewModel.SelectedCity;
                existingTechnician.UpdatedAt = DateTime.Now;
                existingTechnician.UpdatedBy = technicianViewModel.UpdatedBy;
                existingTechnician.IsActive = technicianViewModel.IsActive;

                // Add new customers to the technician, if not already associated
                var customerIds = new List<int>();
                if (technicianViewModel.SelectedCustomerIds != null)
                {
                    foreach (var customerId in technicianViewModel.SelectedCustomerIds)
                    {
                        var customer = await _customerRepo.GetByIdAsync(customerId);
                        if (customer != null && !existingTechnician.TechnicianCustomers.Any(tc => tc.CustomerId == customerId))
                        {
                            existingTechnician.TechnicianCustomers.Add(new TechnicianCustomer { CustomerId = customerId });
                        }
                        customerIds.Add(customerId);
                    }
                }

                // Add new users to the technician, if not already associated
                var userIds = new List<string>();
                if (technicianViewModel.SelectedUserCodes != null)
                {
                    foreach (var userId in technicianViewModel.SelectedUserCodes)
                    {
                        var user = await _userManager.FindByIdAsync(userId); // Use _userManager instead of _userRepo
                        if (user != null && !existingTechnician.TechnicianUsers.Any(tu => tu.UserId == userId))
                        {
                            existingTechnician.TechnicianUsers.Add(new TechnicianUser { UserId = userId });
                        }
                        userIds.Add(userId);
                    }
                }

                // Save the updated technician
                return await _technicianRepo.UpdateAsync(existingTechnician, customerIds, userIds);
            }
            return false;
        }

        public async Task<List<SelectListItem>> GetCustomersForDropdownAsync()
        {
            var customers = await _technicianRepo.GetCustomersForDropdownAsync();

            return customers
                .Where(c => c.IsActive)
                .Select(c => new SelectListItem
                {
                    Value = c.Code,
                    Text = $"{c.Code} - {c.Name}"
                })
                .ToList();
        }

        public async Task<List<SelectListItem>> GetUsersForDropdownAsync()
        {
            try
            {
                // Fetch all active and confirmed users
                var activeConfirmedUsers = await _userManager.Users
                    .Where(user => user.IsActive && user.EmailConfirmed)
                    .ToListAsync();

                // Filter the users by the two new roles
                var representatives = new List<ApplicationUser>();
                foreach (var user in activeConfirmedUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("مندوب تسليم إدارة المخازن") || roles.Contains("مندوب إدارة التسويق وخدمة العملاء")|| roles.Contains("مندوب تسويق وخدمة عملاء"))
                    {
                        representatives.Add(user);
                    }
                }

                // Return the list of SelectListItem
                return representatives.Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = $"{u.UserName}" // Display username
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users for dropdown: {ex.Message}");
                return new List<SelectListItem>();
            }
        }


        private void PopulateDropdowns(TechnicianViewModel viewModel)
        {
            // Hardcoded data for governates and cities
            var governates = new List<SelectListItem>
            {
                new SelectListItem { Text = "Cairo", Value = "Cairo" },
                new SelectListItem { Text = "Giza", Value = "Giza" },
                new SelectListItem { Text = "Alexandria", Value = "Alexandria" }
            };

            var cities = new List<SelectListItem>
            {
                new SelectListItem { Text = "Nasr City", Value = "Nasr City" },
                new SelectListItem { Text = "Dokki", Value = "Dokki" },
                new SelectListItem { Text = "Smouha", Value = "Smouha" }
            };

            viewModel.Governates = governates;
            viewModel.Cities = cities;
        }

        public async Task<bool> ToggleActivationAsync(int technicianId)
        {
            var technician = await _technicianRepo.GetByIdAsync(technicianId);
            if (technician == null)
            {
                return false;
            }

            technician.IsActive = !technician.IsActive;
            technician.UpdatedAt = DateTime.Now;

            // Pass empty lists for customerIds and userIds as they are not needed for this update
            return await _technicianRepo.UpdateAsync(technician, new List<int>(), new List<string>());
        }

        public async Task<bool> ImportTechniciansFromExcelAsync(Stream stream)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                if (worksheet == null)
                    throw new Exception("No worksheet found in the Excel file.");

                var rowCount = worksheet.Dimension.Rows;
                var technicians = new List<Technician>();

                for (int row = 2; row <= rowCount; row++) // Assuming the first row contains headers
                {
                    // Read the data from the Excel file
                    var code = worksheet.Cells[row, 1].Text.Trim(); // Column 1: Code
                    var name = worksheet.Cells[row, 2].Text.Trim(); // Column 2: Name
                    var nickName = worksheet.Cells[row, 3].Text.Trim(); // Column 3: Nickname
                    var nationalID = worksheet.Cells[row, 4].Text.Trim(); // Column 4: National ID
                    var phoneNumber1 = worksheet.Cells[row, 5].Text.Trim(); // Column 5: Phone Number 1
                    var phoneNumber2 = worksheet.Cells[row, 6].Text.Trim(); // Column 6: Phone Number 2
                    var phoneNumber3 = worksheet.Cells[row, 7].Text.Trim(); // Column 7: Phone Number 3
                    var customerNames = worksheet.Cells[row, 8].Text.Trim(); // Column 8: Customer Names (comma-separated)
                    var city = worksheet.Cells[row, 9].Text.Trim(); // Column 9: City
                    var usernames = worksheet.Cells[row, 10].Text.Trim(); // Column 10: Representative Usernames (comma-separated)
                    var governate = worksheet.Cells[row, 11].Text.Trim(); // Column 11: Governorate

                    // Validate mandatory fields
                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(phoneNumber1))
                        continue;

                    // Create a new technician
                    var technician = new Technician
                    {
                        Code = code,
                        Name = name,
                        NickName = string.IsNullOrWhiteSpace(nickName) ? "N/A" : nickName,
                        NationalID = nationalID,
                        PhoneNumber1 = phoneNumber1,  // Stored as string
                        PhoneNumber2 = phoneNumber2,  // Stored as string
                        PhoneNumber3 = phoneNumber3,  // Stored as string
                        City = city,
                        Governate = governate,
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        TechnicianCustomers = new List<TechnicianCustomer>()
                    };

                    // Process customers by names and link them through the relationship table
                    var customerNamesList = string.IsNullOrWhiteSpace(customerNames) ? new List<string>() : customerNames.Split(',').Select(c => c.Trim()).ToList();
                    var customerIds = new List<int>();
                    var customerCodes = new List<string>();

                    if (customerNamesList.Any())
                    {
                        var customers = await _customerRepo.GetCustomersByNamesAsync(customerNamesList);

                        if (!customers.Any())
                        {
                            Console.WriteLine($"No customers found for technician {name} with names: {string.Join(", ", customerNamesList)}");
                        }

                        foreach (var customer in customers)
                        {
                            technician.TechnicianCustomers.Add(new TechnicianCustomer { CustomerId = customer.CustomerID });
                            customerIds.Add(customer.CustomerID);
                            customerCodes.Add(customer.Code);
                        }
                    }

                    // Process representatives and link them through the relationship table
                    var userIds = new List<string>();
                    if (!string.IsNullOrWhiteSpace(usernames))
                    {
                        var usernameList = usernames.Split(',').Select(u => u.Trim()).ToList();
                        var users = await _userManager.Users
                                                      .Where(u => usernameList.Contains(u.UserName))
                                                      .ToListAsync();

                        foreach (var user in users)
                        {
                            technician.TechnicianUsers.Add(new TechnicianUser { UserId = user.Id });
                            userIds.Add(user.Id);
                        }
                    }

                    // Add technician to the list
                    technicians.Add(technician);

                    // Save technician to the database
                    await _technicianRepo.AddAsync(technician, customerCodes.ToList(), userIds);

                    // Debug: Log customer codes associated with the technician
                    if (customerCodes.Any())
                    {
                        Console.WriteLine($"Technician {name} linked to customer codes: {string.Join(", ", customerCodes)}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine(ex.Message);
                return false;
            }
        }



        // Customer Management
        public async Task AssignCustomerAsync(int technicianId, int customerId)
        {
            await _technicianRepo.AssignCustomerAsync(technicianId, customerId);
        }

        public async Task RemoveCustomerByNameAsync(int technicianId, string customerName)
        {
            await _technicianRepo.RemoveCustomerByNameAsync(technicianId, customerName);
        }


        public async Task<List<CustomerViewModel>> GetUnassignedActiveCustomersAsync(int technicianId)
        {
            var unassignedCustomers = await _technicianRepo.GetActiveUnassignedCustomersAsync(technicianId);

            return unassignedCustomers
                .Select(c => new CustomerViewModel
                {
                    CustomerID = c.CustomerID,
                    Name = c.Name,
                    IsActive = c.IsActive
                })
                .ToList();
        }

        // User Management
        // Assign Representative
        public async Task AssignRepresentativeAsync(int technicianId, string userId)
        {
            await _technicianRepo.AssignUserAsync(technicianId, userId);
        }

        // Remove Representative
        public async Task RemoveRepresentativeAsync(int technicianId, string userId)
        {
            await _technicianRepo.RemoveUserByNameAsync(technicianId, userId);
        }

        //Get Active Unassigned Representatives
        public async Task<List<ApplicationUser>> GetActiveUnassignedRepresentativesAsync(int technicianId)
        {
            return await _technicianRepo.GetActiveUnassignedRepresentativesAsync(technicianId);
        }

        // Get Users by Role
        public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName)
        {
            return await _technicianRepo.GetUsersByRoleAsync(roleName);
        }

    }
}
