using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LoyaltyCouponsSystem.DAL.Entity;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace LoyaltyCouponsSystem.BLL.Service.Implementation.GenerateExcel
{
    public class clsGenerateExcelWithCoupons
    {
        public async Task<byte[]> GenerateExcelWithCouponsAsync(
            List<Coupon> coupons)
        {
            // إعداد مكتبة EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Coupons");

                // إعداد الرأس (Header)
                worksheet.Cells[1, 1].Value = "QR Code";
                worksheet.Cells[1, 2].Value = "Serial Number";
                worksheet.Row(1).Style.Font.Bold = true;

                int batchSize = 100; // حجم الدفعة
                int totalBatches = (int)Math.Ceiling((double)coupons.Count / batchSize);

                // تقسيم البيانات إلى دفعات ومعالجتها بشكل متوازي
                await Task.Run(() =>
                {
                    Parallel.For(0, totalBatches, batchIndex =>
                    {
                        int start = batchIndex * batchSize;
                        int end = Math.Min(start + batchSize, coupons.Count);

                        for (int i = start; i < end; i++)
                        {
                            int row = i + 2; // الصف يبدأ من 2 بعد الرأس
                            var coupon = coupons[i];

                            worksheet.Cells[row, 1].Value = coupon.CouponeId;
                            worksheet.Cells[row, 2].Value = coupon.SerialNumber;
                           
                        }
                    });
                });

                // تنسيق الأعمدة
                worksheet.Column(1).Width =40 ; // عرض عمود QR Code
                worksheet.Column(2).Width=20; // ضبط عرض عمود Serial Number 

                // حفظ الملف في الذاكرة
                using (var stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
