using LoyaltyCouponsSystem.BLL.Service.Abstraction;
using LoyaltyCouponsSystem.BLL.ViewModel.Customer;
using LoyaltyCouponsSystem.DAL.Entity;
using LoyaltyCouponsSystem.DAL.Repo.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;

namespace LoyaltyCouponsSystem.BLL.Service.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepo _customerRepo;

        public CustomerService(ICustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<bool> AddAsync(CustomerViewModel customerViewModel)
        {
            if (customerViewModel != null)
            {
                var customer = new Customer
                {
                    CustomerID = customerViewModel.CustomerID,
                    Name = customerViewModel.Name,
                    Code = customerViewModel.Code,
                    Governate = customerViewModel.Governate,
                    City = customerViewModel.City,
                    PhoneNumber = customerViewModel.PhoneNumber,
                    IsActive = customerViewModel.IsActive
                };
                return await _customerRepo.AddAsync(customer);
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id != 0)
            {
                return await _customerRepo.DeleteAsync(id);
            }
            return false;
        }

        public async Task<List<CustomerViewModel>> GetAllAsync()
        {
            var customers = await _customerRepo.GetAllAsync();

            // Manual Mapping and order by CreatedAt descending
            var customerViewModels = customers
                .OrderByDescending(c => c.CreatedAt) // Order by newest (CreatedAt)
                .Select(customer => new CustomerViewModel
                {
                    CustomerID = customer.CustomerID,
                    Name = customer.Name,
                    Code = customer.Code,
                    Governate = customer.Governate,
                    City = customer.City,
                    PhoneNumber = customer.PhoneNumber,
                    CreatedBy = customer.CreatedBy,
                    CreatedAt = customer.CreatedAt,
                    UpdatedBy = customer.UpdatedBy,
                    UpdatedAt = customer.UpdatedAt,
                    IsActive = customer.IsActive,
                })
                .ToList();

            return customerViewModels;
        }

        // Get a customer by id
        public async Task<CustomerViewModel> GetByIdAsync(int id)
        {
            if (id != 0)
            {
                var customer = await _customerRepo.GetByIdAsync(id);

                if (customer != null)
                {
                    return new CustomerViewModel
                    {
                        CustomerID = customer.CustomerID,
                        Name = customer.Name,
                        Code = customer.Code,
                        Governate = customer.Governate,
                        City = customer.City,
                        PhoneNumber = customer.PhoneNumber,
                        //TechnicianID = customer.TechnicianId,
                        IsActive = customer.IsActive
                    };
                }
            }
            return null;
        }

        public async Task<bool> UpdateAsync(UpdateCustomerViewModel updateCustomerViewModel)
        {
            if (updateCustomerViewModel != null && updateCustomerViewModel.CustomerID != 0) 
            {
                // Fetch the customer by ID
                var existingCustomer = await _customerRepo.GetByIdAsync(updateCustomerViewModel.CustomerID);

                if (existingCustomer != null)
                {
                    // Update properties
                    existingCustomer.Name = updateCustomerViewModel.Name;
                    existingCustomer.Code = updateCustomerViewModel.Code;  
                    existingCustomer.Governate = updateCustomerViewModel.Governate;
                    existingCustomer.City = updateCustomerViewModel.City;
                    existingCustomer.PhoneNumber = updateCustomerViewModel.PhoneNumber;
                    //existingCustomer.TechnicianId = updateCustomerViewModel.TechnicianID;
                    existingCustomer.UpdatedAt = DateTime.Now;
                    existingCustomer.UpdatedBy = updateCustomerViewModel.UpdatedBy;
                    existingCustomer.IsActive = updateCustomerViewModel.IsActive;

                    return await _customerRepo.UpdateAsync(existingCustomer); 
                }
            }
            return false;
        }
        public async Task<bool> ToggleActivationAsync(int customerId, bool isActive)
        {
            var customer = await _customerRepo.GetByIdAsync(customerId);
            if (customer == null)
            {
                return false;
            }
            customer.IsActive = isActive;
            return await _customerRepo.UpdateAsync(customer);
        }

        public async Task<bool> ImportCustomersFromExcelAsync(Stream stream)
        {
            try
            {
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                if (worksheet == null)
                    throw new Exception("No worksheet found in the Excel file.");

                var rowCount = worksheet.Dimension.Rows;
                var customers = new List<Customer>();

                for (int row = 2; row <= rowCount; row++) // Assuming first row contains headers
                {
                    var name = worksheet.Cells[row, 1].Text; // Adjust column indices as needed
                    var code = worksheet.Cells[row, 2].Text;
                    var governate = worksheet.Cells[row, 3].Text;
                    var city = worksheet.Cells[row, 4].Text;
                    var phoneNumber = worksheet.Cells[row, 5].Text;

                    if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(phoneNumber))
                    {
                        customers.Add(new Customer
                        {
                            Name = name,
                            Code = code,
                            Governate = governate,
                            City = city,
                            PhoneNumber = phoneNumber,
                            IsActive = true,
                            CreatedAt = DateTime.Now,                        
                        });
                    }
                }

                // Save customers to the database
                foreach (var customer in customers)
                {
                    await _customerRepo.AddAsync(customer);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsCodeUniqueAsync(string code)
        {
            return !await _customerRepo.IsUniqueCode(code);
        }

    }
}
