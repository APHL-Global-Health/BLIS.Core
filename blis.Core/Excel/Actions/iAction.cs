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

using blis.Core.Excel;
using blis.Core.Infrastructure;
using Simplexcel;
using System;
using System.Collections.Generic;

namespace blis.Excel.Actions
{
    public class iAction
    {
        public string Action { get; set; }

        public iAction(string action)
        {
            Action = action;
        }

        public virtual void Process(iExcel owner, ref Workbook book, ref Worksheet sheet)
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
                        if (key.ToLower().Equals("write")) list.Add(new Write(action));
                        else if (key.ToLower().Equals("addsheet")) list.Add(new AddSheet(action));
                        else if (key.ToLower().Equals("output")) list.Add(new Output(action));
                    }
                }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace); }
            }
            return list;
        }
    }
}