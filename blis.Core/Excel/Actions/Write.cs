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

using blis.Core.Excel;
using blis.Core.Infrastructure;
using Simplexcel;
using System;
using blis.Core.Extensions;

namespace blis.Excel.Actions
{
    public class Write : iAction
    {
        public object Content { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public dynamic Style { get; set; }

        public Write(dynamic details)
            :base((string)details["Action"])
        {
            Content = (object)details["Content"];
            X = (int)details["X"];
            Y = (int)details["Y"];
            Style = details["Style"];
        }

        public override void Process(iExcel owner, ref Workbook book, ref Worksheet sheet)
        {
            try
            {
                if (sheet != null && Content != null && X > -1 && Y > -1)
                {
                    sheet.Cells[X, Y].Value = Content.ToString();

                    if (Content.IsNumber()) sheet.Cells[X, Y].Format = BuiltInCellFormat.NumberNoDecimalPlaces;
                    else sheet.Cells[X, Y].Format = BuiltInCellFormat.Text;

                    if (Style != null)
                    {
                        try { if (Style["FontName"] != null) sheet.Cells[X, Y].FontName = (string)Style["FontName"]; } catch { }
                        try { if (Style["Bold"] != null) sheet.Cells[X, Y].Bold = (bool)Style["Bold"]; } catch { }
                        try { if (Style["FontSize"] != null) sheet.Cells[X, Y].FontSize = (int)Style["Bold"]; } catch { }
                        try { if (Style["Format"] != null) sheet.Cells[X, Y].Format = (string)Style["Format"]; } catch { }
                        try { if (Style["Hyperlink"] != null) sheet.Cells[X, Y].Hyperlink = (string)Style["Hyperlink"]; } catch { }
                        try { if (Style["Italic"] != null) sheet.Cells[X, Y].Italic = (bool)Style["Italic"]; } catch { }
                        try { if (Style["Underline"] != null) sheet.Cells[X, Y].Underline = (bool)Style["Underline"]; } catch { }
                        try {
                            if (Style["TextColor"] != null)
                            {
                                var col = System.Drawing.ColorTranslator.FromHtml((string)Style["TextColor"]);
                                sheet.Cells[X, Y].TextColor = col; // Simplexcel.Color.FromArgb(col.A, col.R, col.G, col.B);
                            }
                           
                        } catch { }

                       
                    }
                }
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace); }
        }
    }
}
