using DocumentFormat.OpenXml.Bibliography;
using LoyaltyCouponsSystem.BLL.Service.Abstraction;
using LoyaltyCouponsSystem.BLL.ViewModel.Customer;
using LoyaltyCouponsSystem.BLL.ViewModel.Distributor;
using LoyaltyCouponsSystem.DAL.Entity;
using LoyaltyCouponsSystem.DAL.Repo.Abstraction;
using LoyaltyCouponsSystem.DAL.Repo.Implementation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using Distributor = LoyaltyCouponsSystem.DAL.Entity.Distributor;

namespace LoyaltyCouponsSystem.BLL.Service.Implementation
{
    public class DistributorService : IDistributorService
    {
        private readonly IDistributorRepo _distributorRepo;
        private readonly ICustomerRepo _customerRepo;

        public DistributorService(IDistributorRepo distributorRepo, ICustomerRepo customerRepo)
        {
            _distributorRepo = distributorRepo;
            _customerRepo = customerRepo;
        }

        public async Task<bool> AddAsync(DistributorViewModel distributorViewModel)
        {
            if (distributorViewModel == null)
                return false;

            // Ensure SelectedCustomerCodes is not null
            var selectedCustomerCodes = distributorViewModel.SelectedCustomerCodes ?? new List<string>();

            // Map Distributor entity
            var distributor = new Distributor
            {
                Name = distributorViewModel.Name,
                PhoneNumber1 = distributorViewModel.PhoneNumber1,
                Governate = distributorViewModel.SelectedGovernate,
                City = distributorViewModel.SelectedCity,
                Code = distributorViewModel.Code,
                IsDeleted = distributorViewModel.IsDeleted,
                CreatedBy = distributorViewModel.CreatedBy,
                CreatedAt = distributorViewModel.CreatedAt,
                IsActive = distributorViewModel.IsActive,
                DistributorCustomers = new List<DistributorCustomer>()
            };

            // Fetch Customer IDs by their IDs (not codes)
            var customerIds = selectedCustomerCodes
                .Where(code => int.TryParse(code, out _)) // Only keep valid integer codes
                .Select(code => int.Parse(code))
                .ToList();

            // Fetch the actual Customer objects from the repository (optional, if needed to access full customer data)
            var customers = await _customerRepo.GetCustomersByIdsAsync(customerIds);

            // Create DistributorCustomer mappings
            distributor.DistributorCustomers = customers
                .Select(customer => new DistributorCustomer
                {
                    CustomerID = customer.CustomerID
                })
                .ToList();

            return await _distributorRepo.AddAsync(distributor);
        }




        public async Task<bool> DeleteAsync(int id)
        {
            if (id > 0)
            {
                return await _distributorRepo.DeleteAsync(id);
            }
            return false;
        }

        public async Task<List<DistributorViewModel>> GetAllAsync()
        {
            var distributors = await _distributorRepo.GetAllAsync();
            var allCustomers = await _customerRepo.GetAllAsync();

            return distributors
                .OrderByDescending(d => d.CreatedAt) // Order by latest before mapping
                .Select(d => new DistributorViewModel
                {
                    DistributorID = d.DistributorID,
                    Name = d.Name,
                    SelectedGovernate = d.Governate,
                    SelectedCity = d.City,
                    Code = d.Code,
                    IsDeleted = d.IsDeleted,
                    CreatedAt = d.CreatedAt,
                    PhoneNumber1 = d.PhoneNumber1,
                    CreatedBy = d.CreatedBy,
                    UpdatedBy = d.UpdatedBy,
                    UpdatedAt = d.UpdatedAt,
                    IsActive = d.IsActive,
                    SelectedCustomerNames = d.DistributorCustomers
                        .Where(dc => dc.Customer != null && dc.Customer.IsActive) // Only active customers
                        .Select(x => x.Customer.Name)
                        .Distinct()
                        .ToList(),

                    // Populate AvailableCustomers with only active customers not already assigned to the distributor
                    AvailableCustomers = allCustomers
                        .Where(c => c.IsActive && !d.DistributorCustomers.Any(dc => dc.CustomerID == c.CustomerID)) // Exclude already assigned customers and only active customers
                        .Select(c => new CustomerViewModel
                        {
                            CustomerID = c.CustomerID,
                            Name = c.Name,
                            Code = c.Code
                        })
                        .ToList(),
                })
                .ToList();
        }




        public async Task<DistributorViewModel> GetByIdAsync(int id)
        {
            if (id != 0)
            {
                var distributor = await _distributorRepo.GetByIdAsync(id);
                if (distributor != null)
                {
                    var distributorCustomers = distributor.DistributorCustomers ?? new List<DistributorCustomer>();
                    return new DistributorViewModel
                    {
                        DistributorID = distributor.DistributorID,
                        Name = distributor.Name,
                        PhoneNumber1 = distributor.PhoneNumber1 ,
                        SelectedGovernate = distributor.Governate,
                        SelectedCity = distributor.City,
                        Code = distributor.Code,
                        IsDeleted = distributor.IsDeleted,
                        IsActive = distributor.IsActive,
                        SelectedCustomerNames = distributorCustomers.Where(dc => dc.Customer != null)
                        .Select(x => x.Customer.Name)
                        .Distinct().ToList()
                    };
                }
            }
            return null;
        }


        public async Task<bool> UpdateAsync(UpdateVM distributorViewModel)
        {
            if (distributorViewModel != null)
            {
                // Fetch the existing distributor by ID to ensure it exists
                var existingDistributor = await _distributorRepo.GetByIdAsync(distributorViewModel.DistributorID);

                if (existingDistributor == null)
                {
                    return false;
                }

                // Map to Distributor entity for update
                var distributor = new Distributor
                {
                    DistributorID = distributorViewModel.DistributorID,
                    Name = distributorViewModel.Name,
                    PhoneNumber1 = distributorViewModel.PhoneNumber1,
                    Governate = distributorViewModel.SelectedGovernate,
                    City = distributorViewModel.SelectedCity,
                    Code = distributorViewModel.Code,
                    IsDeleted = distributorViewModel.IsDeleted,
                    UpdatedBy = distributorViewModel.UpdatedBy,
                    UpdatedAt = distributorViewModel.UpdatedAt,
                    IsActive = distributorViewModel.IsActive,
                };

                // Call the repository update method
                return await _distributorRepo.UpdateAsync(distributor);
            }
            return false;
        }


        // Fetch customers for dropdown
        public async Task<List<SelectListItem>> GetCustomersForDropdownAsync()
        {
            var customers = await _distributorRepo.GetCustomersForDropdownAsync();

            return customers.Select(c => new SelectListItem
            {
                Value = c.Code,
                Text = $"{c.Code} - {c.Name}"
            }).ToList();
        }
        public async Task<IEnumerable<SelectListItem>> GetGovernatesForDropdownAsync()
        {
            return new List<SelectListItem>
    {
                new SelectListItem { Text = "Aswan", Value = "Aswan" },
                new SelectListItem { Text = "Assiut", Value = "Assiut" },
                new SelectListItem { Text = "Alexandria", Value = "Alexandria" },
                new SelectListItem { Text = "Ismailia", Value = "Ismailia" },
                new SelectListItem { Text = "Luxor", Value = "Luxor" },
                new SelectListItem { Text = "Red Sea", Value = "Red Sea" },
                new SelectListItem { Text = "Beheira", Value = "Beheira" },
                new SelectListItem { Text = "Giza", Value = "Giza" },
                new SelectListItem { Text = "Dakahlia", Value = "Dakahlia" },
                new SelectListItem { Text = "Suez", Value = "Suez" },
                new SelectListItem { Text = "Sharqia", Value = "Sharqia" },
                new SelectListItem { Text = "Gharbia", Value = "Gharbia" },
                new SelectListItem { Text = "Cairo", Value = "Cairo" },
                new SelectListItem { Text = "Qalyubia", Value = "Qalyubia" },
                new SelectListItem { Text = "New Valley", Value = "New Valley" },
                new SelectListItem { Text = "Matrouh", Value = "Matrouh" },
                new SelectListItem { Text = "Minya", Value = "Minya" },
                new SelectListItem { Text = "Fayoum", Value = "Fayoum" },
                new SelectListItem { Text = "Beni Suef", Value = "Beni Suef" },
                new SelectListItem { Text = "Kafr El Sheikh", Value = "Kafr El Sheikh" },
                new SelectListItem { Text = "Port Said", Value = "Port Said" },
                new SelectListItem { Text = "Damietta", Value = "Damietta" },
                new SelectListItem { Text = "Menoufia", Value = "Menoufia" },
                new SelectListItem { Text = "Qena", Value = "Qena" },
                new SelectListItem { Text = "Sohag", Value = "Sohag" },
                new SelectListItem { Text = "South Sinai", Value = "South Sinai" },
                new SelectListItem { Text = "North Sinai", Value = "North Sinai" }
    };
        }
        public async Task<bool> ToggleActivationAsync(int distributorId)
        {
            var distributor = await _distributorRepo.GetByIdAsync(distributorId);
            if (distributor == null)
            {
                return false;
            }

            // Toggle the IsActive status
            distributor.IsActive = !distributor.IsActive;

            // Save the updated distributor
            return await _distributorRepo.UpdateAsync(distributor);
        }


        public async Task<bool> ImportDistributorsFromExcelAsync(Stream stream)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                if (worksheet == null)
                    throw new Exception("No worksheet found in the Excel file.");

                var rowCount = worksheet.Dimension.Rows;
                var distributors = new List<Distributor>();

                for (int row = 2; row <= rowCount; row++) // Assuming first row contains headers
                {
                    var name = worksheet.Cells[row, 1].Text; // Column 1: Distributor Name
                    var code = worksheet.Cells[row, 2].Text; // Column 2: Distributor Code
                    var governate = worksheet.Cells[row, 3].Text; // Column 3: Governate
                    var city = worksheet.Cells[row, 4].Text; // Column 4: City
                    var phoneNumberText = worksheet.Cells[row, 5].Text; // Column 5: Phone Number
                    var selectedCustomerCodes = worksheet.Cells[row, 6].Text; // Column 6: Customer Codes (comma-separated)
                    var createdAtText = worksheet.Cells[row, 7].Text; // Column 7: CreatedAt (Date)

                    if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(phoneNumberText))
                    {
                        var phoneNumber = phoneNumberText.Trim(); // Keep the phone number as a string

                        // Validate phone number format (if needed)
                        if (!Regex.IsMatch(phoneNumber, @"^01\d{9}$"))
                        {
                            // If phone number is invalid, skip this row or handle the error
                            continue;
                        }

                        // Parse CreatedAt date (default to now if invalid)
                        DateTime createdAt = DateTime.Now;
                        if (!string.IsNullOrWhiteSpace(createdAtText) && DateTime.TryParse(createdAtText, out var parsedDate))
                        {
                            createdAt = parsedDate;
                        }

                        var distributor = new Distributor
                        {
                            Name = name,
                            Code = code,
                            Governate = governate,
                            City = city,
                            PhoneNumber1 = phoneNumber,  // Now stored as a string
                            IsActive = true,
                            CreatedAt = createdAt, // Save the extracted date
                            IsDeleted = false,
                        };

                        // Split the customer codes and get customer IDs from the repository
                        var customerCodes = selectedCustomerCodes.Split(',')
                            .Select(c => c.Trim())
                            .Where(c => !string.IsNullOrEmpty(c)) // Remove empty values
                            .ToList();

                        var customerIds = await _customerRepo.GetCustomerIdsByCodesAsync(customerCodes);

                        // Create DistributorCustomer mappings
                        distributor.DistributorCustomers = customerIds
                            .Select(customerId => new DistributorCustomer
                            {
                                CustomerID = customerId
                            })
                            .ToList();

                        distributors.Add(distributor);
                    }
                }

                // Save distributors to the database
                foreach (var distributor in distributors)
                {
                    await _distributorRepo.AddAsync(distributor);
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging (optional)
                Console.WriteLine(ex.Message);
                return false;
            }
        }



        public async Task<bool> AddCustomerToDistributorAsync(int distributorId, int customerId)
        {
            var distributor = await _distributorRepo.GetDistributorByIdAsync(distributorId);
            var customer = await _distributorRepo.GetCustomerByIdAsync(customerId);

            if (distributor == null || customer == null)
            {
                return false;
            }

            if (!distributor.DistributorCustomers.Any(dc => dc.CustomerID == customerId))
            {
                distributor.DistributorCustomers.Add(new DistributorCustomer
                {
                    DistributorID = distributorId,
                    CustomerID = customerId
                });
                await _distributorRepo.SaveAsync();
                return true;
            }

            return false;
        }


        public async Task<bool> RemoveCustomerFromDistributorByNameAsync(int distributorId, string customerName)
        {
            // Fetch the distributor and its related customers
            var distributor = await _distributorRepo.GetDistributorByIdAsync(distributorId);
            if (distributor == null) return false;

            // Find the specific DistributorCustomer by matching the customer's name
            var distributorCustomer = distributor.DistributorCustomers
                .FirstOrDefault(dc => dc.Customer.Name.Equals(customerName, StringComparison.OrdinalIgnoreCase));

            if (distributorCustomer != null)
            {
                // Remove the customer association
                distributor.DistributorCustomers.Remove(distributorCustomer);
                await _distributorRepo.SaveAsync(); // Save changes to the database
                return true;
            }

            return false; // Customer not found
        }



    }
}
