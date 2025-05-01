using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.ViewModel.ReceiveFromCustomer
{
    public class TransactionPaginationViewModel
    {
        public IEnumerable<ReceiveFromCustomerViewModel> Transactions { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }

}
