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
using System;
using Xilium.CefGlue;

namespace blis.Linux.Extensions.V8.CefGlue
{
    public class ConsoleObject : CefV8Handler
    {
        #region Variables
        private CefV8Value Handler;
        #endregion

        #region Constructor
        public ConsoleObject(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
        }
        #endregion

        #region CreateObject
        public CefV8Value CreateObject()
        {
            Handler = CefV8Value.CreateObject();
            Handler.SetValue("log", CefV8Value.CreateFunction("log", this), CefV8PropertyAttribute.None);
            return Handler;
        }
        #endregion

        #region Execute
        protected override bool Execute(string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
        {
            returnValue = CefV8Value.CreateNull();
            exception = null;

            try
            {
                var context = CefV8Context.GetCurrentContext();
                var browser = context.GetBrowser();
                var frame = browser.GetMainFrame();

                if (name == "log" && arguments.Length > 0)
                {
                    var message = arguments[0].GetStringValue();
                    if (!string.IsNullOrEmpty(message))
                        Log.Trace(message.ToString(), true);
                }
                return true;
            }
            catch (Exception ex)
            {
                returnValue = CefV8Value.CreateNull();
                exception = ex.Message;
                return true;
            }
        }
        #endregion
    }
}
