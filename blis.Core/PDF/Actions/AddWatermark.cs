// --------------------------------------------------------------------------------------------------------------------
//<license>
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// blis project is licensed under MIT License. CefGlue may have additional licensing.
// This is a port from CefGlue.WindowsForms sample of CefGlue. Mostly provided as-is. 
// For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
// </note>
// --------------------------------------------------------------------------------------------------------------------

#region Using
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
#endregion

namespace blis.Core.PDF.Actions
{
    public class AddWatermark : iAction
    {
        public string Data { get; set; }

        public AddWatermark(dynamic details)
            :base((string)details["Action"])
        {
            Data = (string)details["Text"];
        }



        public override void Process(iPDF owner, ref PdfDocument Document, ref PdfPage Page, ref XGraphics Graphics)
        {
            if (!string.IsNullOrEmpty(Data))
            {
                var watermark = Data;
                XSize size = Graphics.MeasureString(watermark, owner.Font);

                // Define a rotation transformation at the center of the page
                Graphics.TranslateTransform(Page.Width / 2, Page.Height / 2);
                Graphics.RotateTransform(-Math.Atan(Page.Height / Page.Width) * 180 / Math.PI);
                Graphics.TranslateTransform(-Page.Width / 2, -Page.Height / 2);

                // Create a graphical path
                XGraphicsPath path = new XGraphicsPath();

                // Add the text to the path
                path.AddString(watermark, owner.Font.FontFamily, XFontStyle.BoldItalic, 150, new XPoint(0, (Page.Height / 2) - 80), XStringFormats.TopLeft);

                // Create a dimmed red pen and brush
                XPen pen = new XPen(XColor.FromArgb(50, 75, 0, 130), 3);
                XBrush brush = new XSolidBrush(XColor.FromArgb(50, 106, 90, 205));

                // Stroke the outline of the path
                Graphics.DrawPath(pen, brush, path);
            }
        }
    }
}
