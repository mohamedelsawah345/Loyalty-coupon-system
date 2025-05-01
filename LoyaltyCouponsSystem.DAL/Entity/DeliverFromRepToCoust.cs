using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class DeliverFromRepToCoust
    {
        

        [Key]
        public int Id { get; set; }


        public string? RepresintitiveCode { get; set; }

        public int? GovernorateId { get; set; }
        public Governorate? Governorates { get; set; }
        public Area? Areas { get; set; }
        public int? AreaId { get; set; }
        

        public byte[] Image { get; set; }  // Store the image as byte array
        public DateTime Timestamp { get; set; }= DateTime.Now;
       
        public int? CustomerID { get; set; }
        public Customer? Customer { get; set; }
        public int? TechnicianID { get; set; }
        public Technician? Technician { get; set; }
      
        public string ExchangePermission { get; set; }
        public string? CreatedBy { get; set; }
        public string? CostomerCode { get; set; }
        public string? TechnitionCode { get; set; }
    }
}
