using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xceed.Document.NET;
using System.Linq;
using Xceed.Words.NET;

namespace LoyaltyCouponsSystem.BLL.Service.Implementation.GenerateWord
{
    public class clsGenerateWordWithCoupons
    {
        public async Task<byte[]> GenerateWordWithCouponsAsync(
            List<byte[]> couponImages,
            int couponsPerRow,
            float horizontalSpacingCm,
            float verticalSpacingCm,
            float couponSizeCm)
        {
            // تحويل cm إلى px (بناءً على دقة الطباعة الافتراضية 96 DPI)
            float ConvertCmToPx(float cm) => cm * (96f / 2.54f);

            float couponSizePx = ConvertCmToPx(couponSizeCm);
            float horizontalSpacingPx = ConvertCmToPx(horizontalSpacingCm);
            float verticalSpacingPx = ConvertCmToPx(verticalSpacingCm);

            // إنشاء مستند Word جديد
            using (DocX document = DocX.Create("Coupons.docx"))
            {
                // إضافة جدول جديد بعدد الأعمدة الذي تريده
                var table = document.AddTable(1, couponsPerRow);
                table.AutoFit = AutoFit.ColumnWidth; // جعل الجدول يتناسب مع الأعمدة

                int currentColumn = 0;

                // التعامل مع الصور بطريقة غير متزامنة
                foreach (var imageData in couponImages)
                {
                    // تحويل الصورة إلى MemoryStream
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        // إدراج الصورة
                        var image = document.AddImage(ms);
                        var picture = image.CreatePicture((int)couponSizePx, (int)couponSizePx);

                        // إضافة الصورة إلى الخلية الحالية
                        var cell = table.Rows[table.RowCount - 1].Cells[currentColumn];
                        cell.Paragraphs.First().AppendPicture(picture).Alignment = Alignment.center; // تعديل المحاذاة

                        // الانتقال إلى العمود التالي
                        currentColumn++;

                        // إذا انتهى الصف الحالي، أضف صفًا جديدًا
                        if (currentColumn >= couponsPerRow)
                        {
                            currentColumn = 0;
                            table.InsertRow(); // إضافة صف جديد
                        }
                    }
                }

                // إضافة مسافات بين الصفوف (يمكنك إضافة المزيد من التنسيق حسب الحاجة)
                table.SetBorder(TableBorderType.Bottom, new Border(BorderStyle.Tcbs_none, BorderSize.one, 0, System.Drawing.Color.Transparent));
                table.Rows.ForEach(row => row.Height = verticalSpacingPx); // تحديد ارتفاع الصفوف

                // إدراج الجدول في المستند
                document.InsertTable(table);

                // حفظ المستند في الذاكرة
                using (MemoryStream stream = new MemoryStream())
                {
                    document.SaveAs(stream);
                    return stream.ToArray(); // إرجاع المستند كـ بايت
                }
            }
        }
    }
}