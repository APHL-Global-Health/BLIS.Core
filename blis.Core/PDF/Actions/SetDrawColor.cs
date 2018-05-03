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
#endregion

namespace blis.Core.PDF.Actions
{
    public class SetDrawColor : iAction
    {
        public int Ch1 { get; set; }
        public int Ch2 { get; set; }
        public int Ch3 { get; set; }
        public int Ch4 { get; set; }
       
        public SetDrawColor(dynamic details)
            :base((string)details["Action"])
        {
            Ch1 = details["Ch1"] != null ? (int)details["Ch1"] : 0;
            Ch2 = details["Ch2"] != null ? (int)details["Ch2"] : 0;
            Ch3 = details["Ch3"] != null ? (int)details["Ch3"] : 0;
            Ch4 = details["Ch4"] != null ? (int)details["Ch4"] : 0;
        }
        
        public override void Process(iPDF owner, ref PdfDocument Document, ref PdfPage Page, ref XGraphics Graphics)
        {
            owner.TextColor = XColor.FromArgb(255, Ch1, Ch2, Ch3);
        }
    }
}
