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
using blis.Core.PDF.Actions;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace blis.Core.PDF
{
    public class iPDF
    {
        //PdfDocument Document = null;
        public Internal Internal { get; set; }
        public Properties Properties { get; set; }
        public List<iAction> Actions { get; set; }
        
        public XFont    Font { get; set; }
        public XColor   TextColor { get; set; }
        public XColor   FillColor { get; set; }
        public double   LineWidth { get; set; }
        public string   Project { get; set; }
        public string   Directory { get; set; }

        public iPDF(string project, string directory, dynamic pdf)
        {
            this.Project = project;
            this.Directory = directory;
            
            this.Internal    = new Internal(pdf["internal"]);
            this.Properties  = new Properties(pdf["Properties"]);
            this.Actions     = iAction.Parse(pdf["Actions"]);
            this.TextColor = XColors.Black;
            this.FillColor = XColors.White;
            this.Font = new XFont("Arial", 8, XFontStyle.Regular);
        }

        public static byte[] CreatePDF(iPDF owner)
        {
            var bytes = new byte[0];
            var Document = new PdfDocument();
            Document.Info.Title = owner.Properties.Title;

            var Page = Document.AddPage();
            var Graphics = XGraphics.FromPdfPage(Page);

            foreach(var action in owner.Actions)
            {
                action.Process(owner, ref Document, ref Page, ref Graphics);
            }

            try
            {
                if (Document != null && Document.PageCount > 0)
                {
                    owner.Properties.Items = Document.PageCount;
                    using (var ms = new MemoryStream())
                    {
                        Document.Save(ms, false);
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
