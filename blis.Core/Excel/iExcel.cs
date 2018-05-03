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

using blis.Core.Infrastructure;
using blis.Excel;
using blis.Excel.Actions;
using Simplexcel;
using System;
using System.Collections.Generic;
using System.IO;

namespace blis.Core.Excel
{
    public class iExcel
    {
        public Properties Properties { get; set; }
        public List<iAction> Actions { get; set; }

        public string Project { get; set; }
        public string Directory { get; set; }

        public iExcel(string project, string directory, dynamic excel)
        {
            this.Project = project;
            this.Directory = directory;

            this.Properties = new Properties(excel["Properties"]);
            this.Actions = iAction.Parse(excel["Actions"]);
        }

        public static byte[] CreateExcel(iExcel owner)
        {
            var bytes = new byte[0];
            var Document = new Workbook();
            Document.Title = owner.Properties.Title;
            Document.Author = "blis";

            Worksheet sheet = null;
            foreach (var action in owner.Actions) { action.Process(owner, ref Document, ref sheet);  }

            try
            {
                if (Document != null && owner.Actions.Count > 0)
                {
                    foreach(var _sheet in Document.Sheets) owner.Properties.Items += _sheet.Cells.RowCount;
                    using (var ms = new MemoryStream())
                    {
                        Document.Save(ms, CompressionLevel.Maximum);
                        ms.Position = 0;
                        bytes = ms.ToArray();
                    }
                }
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace); }

            return bytes;
        }
    }
}
