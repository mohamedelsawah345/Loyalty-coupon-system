using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using LoyaltyCouponsSystem.BLL.Service.Abstraction;
using LoyaltyCouponsSystem.DAL.Entity;
using QRCoder;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.Service.Implementation.GenerateQR
{
    public class QRCodeGeneratorHelper : IQRCodeGeneratorHelper
    {
        public async Task<byte[]> GenerateQRCodeAsync(Coupon details, string baseUrl)
        {
            // Build TrackUrl ==> This Url which open when scan QRCode
            string trackUrl = $"{baseUrl}/HistoryScan/track?ID={details.CouponeId}";

            // Generate QR Code
            byte[] combinedImageBytes = new byte[0];
            if (!string.IsNullOrEmpty(trackUrl))
            {
                QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
                QRCodeData data = qrCodeGenerator.CreateQrCode(trackUrl, QRCodeGenerator.ECCLevel.Q);
                BitmapByteQRCode bitmap = new BitmapByteQRCode(data);

                // Generate QR code image
                byte[] qrCodeBytes = bitmap.GetGraphic(20);
                using (var qrCodeImage = new Bitmap(new MemoryStream(qrCodeBytes)))
                {
                    // Define the dimensions for the combined image (QR + text)
                    int combinedWidth = qrCodeImage.Width+125;
                    int combinedHeight = qrCodeImage.Height + 150; // Extra height for the serial number text

                    using (var combinedImage = new Bitmap(combinedWidth, combinedHeight))
                    using (var graphics = Graphics.FromImage(combinedImage))
                    {
                        graphics.Clear(Color.White);

                        // Draw the QR code
                        graphics.DrawImage(qrCodeImage, 0, 0);

                        // Draw the serial number below the QR code
                        string serialNumber = details.SerialNumber.ToString();
                        float targetWidthInCm = 4; // عرض النص المطلوب بالسنتيمتر
                        int fontSize = QRCodeWithTextHelper.GetFontSizeForWidth(graphics, serialNumber, "Arial", targetWidthInCm);

                        using (var font = new Font("Arial", 65, FontStyle.Bold))
                        using (var brush = new SolidBrush(Color.Black))
                        {
                            SizeF textSize = graphics.MeasureString(serialNumber, font);
                            float textX = (combinedWidth - textSize.Width) / 2; // Center the text
                            float textY = qrCodeImage.Height + 3; // Position below the QR code
                            graphics.DrawString(serialNumber, font, brush, textX, textY);
                        }

                        // Save the combined image to a memory stream
                        using (var memoryStream = new MemoryStream())
                        {
                            combinedImage.Save(memoryStream, ImageFormat.Png);
                            combinedImageBytes = memoryStream.ToArray();
                        }
                    }
                }
            }

            return combinedImageBytes;
        }
    }
}
