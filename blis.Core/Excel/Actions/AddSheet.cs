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

namespace blis.Excel.Actions
{
    public class AddSheet : iAction
    {
        public string Title { get; set; }
        public dynamic Style { get; set; }

        public AddSheet(dynamic details)
            :base((string)details["Action"])
        {
            Title = (string)details["Title"];
            Style = details["Style"];
        }

        public override void Process(iExcel owner, ref Workbook book, ref Worksheet sheet)
        {
            try
            {
                if (book != null && !string.IsNullOrEmpty(Title))
                {
                    sheet = new Worksheet(Title);
                    //sheet.PageSetup.PrintRepeatRows = 2;
                    //sheet.PageSetup.PrintRepeatColumns = 2;

                    if (Style != null)
                    {
                        if (Style["Orientation"] != null)
                        {
                            if (((string)Style["Orientation"]).ToLower() == "landscape") sheet.PageSetup.Orientation = Orientation.Landscape;
                            else if (((string)Style["Orientation"]).ToLower() == "portrait") sheet.PageSetup.Orientation = Orientation.Portrait;
                        }
                    }
                    book.Add(sheet);
                }
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace); }
        }
    }
}
