using LoyaltyCouponsSystem.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.Service.Implementation.GenerateQR
{
    public class GenerateListOfCoupons
    {
        public async Task<List<byte[]>> GenerateQRCodesAsync(List<Coupon> coupons, string BaseURL)
        {
            List<byte[]> qrCodes = new List<byte[]>();

            foreach (var coupon in coupons)
            {
                QRCodeGeneratorHelper qRCodeGeneratorHelper = new QRCodeGeneratorHelper();

                // استبدال الكود المتزامن بكود غير متزامن
                byte[] qrCode = await qRCodeGeneratorHelper.GenerateQRCodeAsync(coupon, BaseURL);
                qrCodes.Add((qrCode));
            }

            return qrCodes;
        }
    }
}
