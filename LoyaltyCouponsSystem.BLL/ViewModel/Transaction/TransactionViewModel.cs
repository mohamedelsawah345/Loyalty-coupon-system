using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.ViewModel.Transaction
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public string ExchangePermission { get; set; }
        public LoyaltyCouponsSystem.DAL.Entity.Customer Customer { get; set; } // Include full Customer object
        public string CustomerName => Customer?.Name; // You can include a convenience property to access the name directly
        public LoyaltyCouponsSystem.DAL.Entity.Distributor Distributor { get; set; } // Include full Distributor object
        public string DistributorName => Distributor?.Name; // Convenience property for Distributor name
        public string GovernorateName { get; set; }
        public string AreaName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Timestamp { get; set; }
    }



}
