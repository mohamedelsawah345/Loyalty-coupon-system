using LoyaltyCouponsSystem.DAL.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.ViewModel.DeliverFormRepToCoust
{
    public class DeliverFromRepToCoustVM
    {
       


        [Required(ErrorMessage = "Customer is required.")]
        public string? SelectedCustomerCode { get; set; }

      

        [Display(Name = "Governorate")]
        public int? GovernorateId { get; set; }



        public IEnumerable<Governorate>? governorates { get; set; }

        [Display(Name = "Area")]

        public int? AreaId { get; set; }
        public IEnumerable<Area>? Areas { get; set; } = new List<Area>();




        [Required(ErrorMessage = "Exchange permission is required.")]
        [StringLength(50, ErrorMessage = "Exchange permission cannot exceed 50 characters.")]
        [UniqueExchangePermission(ErrorMessage = "Exchange permission must be unique.")]
        public string ExchangePermission { get; set; }
        public string? CustomerDetails { get; set; }
        public string? TechnicianDetails { get; set; }
        public string? CreatedBy { get; set; }
        public IFormFile image { get; set; } 


    }
}
