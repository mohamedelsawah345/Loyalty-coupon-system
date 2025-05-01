using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.ViewModel.Permission
{
    public class RolePermissionsViewModel
    {
        public string RoleName { get; set; }
        public List<string> AvailablePermissions { get; set; }
        public List<string> AssignedPermissions { get; set; }
    }

}
