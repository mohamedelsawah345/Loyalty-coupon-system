using Microsoft.AspNetCore.Identity;

namespace Loyality_Copoun_System.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}
