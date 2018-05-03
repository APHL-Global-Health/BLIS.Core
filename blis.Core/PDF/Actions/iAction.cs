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
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;

namespace blis.Core.PDF.Actions
{
    public class iAction
    {
        public string Action { get; set; }

        public iAction(string action)
        {
            Action = action;
        }

        public virtual void Process(iPDF owner, ref PdfDocument Document, ref PdfPage Page, ref XGraphics Graphics)
        {
           
        }
        
        public static List<iAction> Parse(dynamic actions)
        {
            var list = new List<iAction>();
            foreach (var action in actions)
            {
                try
                {
                    var key = action["Action"];
                    if (!string.IsNullOrEmpty(key))
                    {
                        if (key.ToLower().Equals("setfontsize")) list.Add(new SetFontSize(action));
                        else if (key.ToLower().Equals("setfonttype")) list.Add(new SetFontType(action));
                        else if (key.ToLower().Equals("text")) list.Add(new Text(action));
                        else if (key.ToLower().Equals("textwithwrap")) list.Add(new TextWithWrap(action));
                        else if (key.ToLower().Equals("setdrawcolor")) list.Add(new SetDrawColor(action));
                        else if (key.ToLower().Equals("setlinewidth")) list.Add(new SetLineWidth(action));
                        else if (key.ToLower().Equals("rect")) list.Add(new Rect(action));
                        else if (key.ToLower().Equals("addimage")) list.Add(new AddImage(action));
                        else if (key.ToLower().Equals("splittexttosize")) list.Add(new SplitTextToSize(action));
                        else if (key.ToLower().Equals("line")) list.Add(new Line(action));
                        else if (key.ToLower().Equals("addpage")) list.Add(new AddPage(action));
                        else if (key.ToLower().Equals("addwatermark")) list.Add(new AddWatermark(action));
                        else if (key.ToLower().Equals("circle")) list.Add(new Circle(action));
                        else if (key.ToLower().Equals("ellipse")) list.Add(new Ellipse(action));
                        else if (key.ToLower().Equals("output")) list.Add(new Output(action));
                    }
                }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace); }
                
            }
            return list;
        }
    }
}
