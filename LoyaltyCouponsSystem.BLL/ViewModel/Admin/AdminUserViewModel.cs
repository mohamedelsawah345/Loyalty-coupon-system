using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.ViewModel.Admin
{
    public class AdminUserViewModel
    {
        public string Id { get; set; }
        public string? Password { get; set; }

        public string UserName { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string NationalID { get; set; }
        public string PhoneNumber { get; set; }
        public string? OptionalPhoneNumber { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Role { get; set; }
        public IEnumerable<string>? Roles { get; set; } 
        public string? SelectedRole { get; set; } 
        public string? FullName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } 

    }
    public class ManageUsersViewModel
    {
        public List<AdminUserViewModel> ConfirmedUsers { get; set; } = new();
        public List<AdminUserViewModel> UnconfirmedUsers { get; set; } = new();
        public List<string> AllRoles { get; set; } = new();
    }
}
