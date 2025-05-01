using Microsoft.AspNetCore.Identity;
using LoyaltyCouponsSystem.DAL.Entity.Permission;

namespace LoyaltyCouponsSystem.BLL.ViewModel.Admin

{
    public class AssignPermissionsToRoleViewModel
    {
        public string RoleName { get; set; }
        public List<string> SelectedPermissions { get; set; }
        

    }

}
