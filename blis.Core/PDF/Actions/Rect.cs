﻿// --------------------------------------------------------------------------------------------------------------------
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
#endregion

namespace blis.Core.PDF.Actions
{
    public class Rect : iAction
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double W { get; set; }
        public double H { get; set; }
        public string Style  { get; set; }

        public Rect(dynamic details)
            :base((string)details["Action"])
        {
            X = (double)details["X"];
            Y = (double)details["Y"];
            W = (double)details["W"];
            H = (double)details["H"];
            Style = (string)details["Style"];
        }

        public override void Process(iPDF owner, ref PdfDocument Document, ref PdfPage Page, ref XGraphics Graphics)
        {
            Graphics.DrawRectangle(new XPen(owner.TextColor), new XSolidBrush(owner.FillColor), X, Y, W, H);
        }
    }
}