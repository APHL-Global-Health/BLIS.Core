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
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
#endregion

namespace blis.Core.PDF.Actions
{
    public class Text : iAction
    {
        public string Data { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Flags { get; set; }

        public Text(dynamic details)
            :base((string)details["Action"])
        {
            Data = (string)details["Text"];
            X = (double)details["X"];
            Y = (double)details["Y"];
            Flags = (string)details["Flags"];
        }

        public override void Process(iPDF owner, ref PdfDocument Document, ref PdfPage Page, ref XGraphics Graphics)
        {
            if (!string.IsNullOrEmpty(Data))
            {
                var tf = new XTextFormatter(Graphics);
                tf.Alignment = XParagraphAlignment.Left;
                var textSize = Graphics.MeasureString(Data, owner.Font);

                if (!string.IsNullOrEmpty(Flags))
                {
                    if (Flags == "center")
                    {
                        tf.Alignment = XParagraphAlignment.Center;
                        X = X - (textSize.Width / 2);
                    }
                    else if (Flags == "default") tf.Alignment = XParagraphAlignment.Default;
                    else if (Flags == "justify") tf.Alignment = XParagraphAlignment.Justify;
                    else if (Flags == "left") tf.Alignment = XParagraphAlignment.Left;
                    else if (Flags == "right")
                    {
                        tf.Alignment = XParagraphAlignment.Right;
                        X = X - textSize.Width;
                    }
                }

                var w = Graphics.MeasureString(Data, owner.Font);
                tf.DrawString(Data, owner.Font, new XSolidBrush(owner.TextColor), new XRect(X, Y, textSize.Width, textSize.Height), XStringFormats.TopLeft);
            }
        }
    }
}
