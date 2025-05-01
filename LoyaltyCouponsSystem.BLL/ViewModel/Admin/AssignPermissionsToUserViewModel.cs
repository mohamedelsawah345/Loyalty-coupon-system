using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.ViewModel.Admin
{
    public class AssignPermissionsToUserViewModel
    {
        public string UserId { get; set; }
        public List<string> SelectedPermissions { get; set; }
    }
}
