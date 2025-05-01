using System.ComponentModel.DataAnnotations;

namespace LoyaltyCouponsSystem.BLL.ViewModel.Account
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }
}

