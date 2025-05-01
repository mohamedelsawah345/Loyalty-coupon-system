using LoyaltyCouponsSystem.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.Service.Abstraction
{
    public interface IQRCodeGeneratorHelper
    {
        // This method generates a QR Code asynchronously.
        // It takes details of the QR Code (like Id, region, year, number in the year) and returns the QR Code as a byte array.
        // The QR Code contains a URL that when scanned opens a page.
        // This URL is the BaseUrl + Id.
        Task<byte[]> GenerateQRCodeAsync(Coupon Details, string baseUrl);
    }
}
