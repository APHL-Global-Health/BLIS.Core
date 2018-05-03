
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
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace blis.Core.PDF.Actions
{
    public class TextWithWrap : iAction
    {
        public string Data { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Size { get; set; }
        public XRect Border { get; set; }
        public bool HasBorder { get; set; }

        public TextWithWrap(dynamic details)
            : base((string)details["Action"])
        {
            Data = (string)details["Text"];
            X = (double)details["X"];
            Y = (double)details["Y"];
            Size = (double)details["Size"];
            Border = new XRect(0, 0, 0, 0);
            HasBorder = false;

            var border = details["Border"];
            if (border != null)
            {
                Border = new XRect((double)border.X, (double)border.Y, (double)border.W, (double)border.H);
                HasBorder = true;
            }
        }

        #region GetSplittedLineCount
        /// <summary>
        /// Calculate the number of soft line breaks
        /// </summary>
        public static int GetSplittedLineCount(XGraphics gfx, string content, XFont font, double maxWidth)
        {
            //handy function for creating list of string
            Func<string, IList<string>> listFor = val => new List<string> { val };
            // string.IsNullOrEmpty is too long :p
            Func<string, bool> nOe = str => string.IsNullOrEmpty(str);
            // return a space for an empty string (sIe = Space if Empty)
            Func<string, string> sIe = str => nOe(str) ? " " : str;
            // check if we can fit a text in the maxWidth
            Func<string, string, bool> canFitText = (t1, t2) => gfx.MeasureString($"{(nOe(t1) ? "" : $"{t1} ")}{sIe(t2)}", font).Width <= maxWidth;

            Func<IList<string>, string, IList<string>> appendtoLast =
                    (list, val) => list.Take(list.Count - 1)
                                       .Concat(listFor($"{(nOe(list.Last()) ? "" : $"{list.Last()} ")}{sIe(val)}"))
                                       .ToList();

            var splitted = content.Split(' ');

            var lines = splitted.Aggregate(listFor(""),
                    (lfeed, next) => canFitText(lfeed.Last(), next) ? appendtoLast(lfeed, next) : lfeed.Concat(listFor(next)).ToList(),
                    list => list.Count());

            return lines;
        }
        #endregion

        public override void Process(iPDF owner, ref PdfDocument Document, ref PdfPage Page, ref XGraphics Graphics)
        {

            var textSize = Graphics.MeasureString(Data, owner.Font);
            int lines = GetSplittedLineCount(Graphics, Data, owner.Font, Size);
            if (HasBorder) Graphics.DrawRectangle(new XPen(owner.TextColor), new XSolidBrush(owner.FillColor), Border.X, Border.Y, Border.Width, (textSize.Height + 2) * lines);

            var tf = new XTextFormatter(Graphics);
            tf.DrawString(Data, owner.Font, new XSolidBrush(owner.TextColor), new XRect(X, Y, Size, (textSize.Height + 2) * lines), XStringFormats.TopLeft);
        }
    }
}
