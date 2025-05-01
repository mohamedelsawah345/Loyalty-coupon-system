using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.Service.Implementation.GenerateQR
{
    public class QRCodeWithTextHelper
    {
        public static int GetFontSizeForWidth(Graphics graphics, string text, string fontName, float targetWidthInCm, float dpi = 96f)
        {
            // Convert target width from cm to pixels
            float targetWidthInPixels = targetWidthInCm * 0.3937f * dpi;

            // Start with a reasonable font size
            int fontSize = 10;

            while (true)
            {
                using (var font = new Font(fontName, fontSize, FontStyle.Bold))
                {
                    SizeF textSize = graphics.MeasureString(text, font);

                    if (textSize.Width >= targetWidthInPixels)
                    {
                        return fontSize; // Return font size that matches the width
                    }

                    fontSize++; // Increment font size and try again
                }
            }
        }
    }
}
