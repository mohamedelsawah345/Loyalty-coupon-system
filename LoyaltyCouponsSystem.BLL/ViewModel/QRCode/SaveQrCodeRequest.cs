using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.ViewModel.QRCode
{
    public class SaveQrCodeRequest
    {
        public string QrCodeData { get; set; }
        public string ReceiptNumber { get; set; }
    }

}
