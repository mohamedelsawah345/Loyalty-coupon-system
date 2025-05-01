using LoyaltyCouponsSystem.DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.ViewModel.DeliverFormRepToCoust
{
    public class TransactionForRecieptFromRepToCustVM
    {

        public int Id { get; set; }
        public string CustomerCode { get; set; }

        public string? TechnitionCode { get; set; }

        public string? ReprsentitiveCode { get; set; }

        public string ExchangePermissionNumber { get; set; }




        public string? GovernorateName { get; set; }

        public string? AreaName { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? GeneratedBy { get; set; }
        public string? ImagePath { get; set; }
        public byte[] Image { get; set; }  // Store the image as byte array
    }
}
