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

using blis.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Xilium.CefGlue;

namespace blis.Windows.Extensions.V8.CefGlue
{
    public class xhrObject : CefV8Handler
    {
        #region Variables
        //private CefV8Value Handler;
        private HttpWebRequest _req;
        private HttpWebResponse _rsp;
        #endregion

        #region Constructor
        public xhrObject(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
        }
        #endregion
        
        #region CreateObject
        public CefV8Value CreateObject()
        {
            CefV8Value Handler = CefV8Value.CreateObject();
            Handler.SetValue("Create", CefV8Value.CreateFunction("Create", this), CefV8PropertyAttribute.None);
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

                if (name == "Create")
                {
                    var Handler = CefV8Value.CreateObject();
                    Handler.SetValue("open", CefV8Value.CreateFunction("open", this), CefV8PropertyAttribute.None);
                    Handler.SetValue("send", CefV8Value.CreateFunction("send", this), CefV8PropertyAttribute.None);
                    Handler.SetValue("onreadystatechange", CefV8Value.CreateFunction("onreadystatechange", this), CefV8PropertyAttribute.None);
                    Handler.SetValue("setRequestHeader", CefV8Value.CreateFunction("setRequestHeader", this), CefV8PropertyAttribute.None);
                    Handler.SetValue("getResponseHeader", CefV8Value.CreateFunction("getResponseHeader", this), CefV8PropertyAttribute.None);
                    returnValue = Handler;
                }
                else if (name == "open")
                {
                    if (arguments.Length >= 2)
                    {
                        var method = arguments[0].GetStringValue();
                        var url = arguments[1].GetStringValue();

                        _req = (HttpWebRequest)WebRequest.Create(arguments[1].GetStringValue());
                        _req.Method = method;
                    }
                }
                else if (name == "send")
                {
                    string data = null;

                    try
                    {
                        if (_req.Method == "POST" && arguments.Length == 1)
                        {
                            if (arguments[0].IsString)
                            {
                                var pd = Encoding.UTF8.GetBytes(arguments[0].GetStringValue());
                                _req.GetRequestStream().Write(pd, 0, pd.Length);
                            }
                            else if (arguments[0].IsArray)
                            {
                                var result = arguments[0];
                                var values = new List<char>();
                                for (int i = 0; i < result.GetArrayLength(); i++)
                                {
                                    var value = result.GetValue(i);
                                    values.Add(Extension.ConvertToV8Value(value));
                                }

                                var pd = values.Select(c => (byte)c).ToArray();
                                _req.GetRequestStream().Write(pd, 0, pd.Length);
                            }
                        }

                        _rsp = (HttpWebResponse)_req.GetResponse();
                        if (_rsp.StatusCode == HttpStatusCode.OK)
                        {
                            var d = new StreamReader(_rsp.GetResponseStream()).ReadToEnd();
                            if (!string.IsNullOrEmpty(d))
                            {
                                data = d;
                            }
                        }
                    }
                    catch {}

                    if (data != null)
                        returnValue = CefV8Value.CreateString(data);
                }
                else if (name == "setRequestHeader")
                {
                    if (arguments.Length == 2)
                    {
                        if (_req != null)
                        {
                            var _name = arguments[0].GetStringValue();
                            var val = arguments[1].GetStringValue();

                            if (!string.IsNullOrEmpty(_name) && !string.IsNullOrEmpty(val))
                            {
                                if (_name.ToLower() == "user-agent") _req.UserAgent = val;
                                else if (_name.ToLower() == "connection") _req.KeepAlive = val.ToLower() == "keep-alive";
                                else _req.Headers.Add(_name, val);
                            }
                        }
                    }
                }
                else if (name == "getResponseHeader")
                {
                    string val = null;
                    if (arguments.Length == 1 && _rsp != null)
                        val = _rsp.Headers[arguments[0].GetStringValue()];

                    if (val != null)
                        returnValue = CefV8Value.CreateString(val);
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
