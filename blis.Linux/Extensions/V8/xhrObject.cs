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
using blis.Core.Configurations;
using blis.Core.Extensions;
using blis.Core.Helpers;
using blis.Core.Infrastructure;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using V8.Net;
using Xilium.CefGlue;
#endregion

namespace blis.Linux.Extensions.V8
{
    public class xhrObject : V8NativeObject
    {
        private HttpWebRequest _req;
        private HttpWebResponse _rsp;

        public override InternalHandle Initialize(bool isConstructCall, params InternalHandle[] args)
        {
            base.Initialize(isConstructCall, args);

            this.AsDynamic.open = Engine.CreateFunctionTemplate().GetFunctionObject(XHROpen);
            this.AsDynamic.send = Engine.CreateFunctionTemplate().GetFunctionObject(XHRSend);
            this.AsDynamic.onreadystatechange = new Action<object>((o) => { });
            this.AsDynamic.setRequestHeader = Engine.CreateFunctionTemplate().GetFunctionObject(XHRAddHeader);
            this.AsDynamic.getResponseHeader = Engine.CreateFunctionTemplate().GetFunctionObject(XHRGetHeader);

            return base.Initialize(isConstructCall, args);
        }

        private InternalHandle XHRGetHeader(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            string val = null;

            if (args.Length == 1 && _rsp != null)
            {
                val = _rsp.Headers[args[0].AsString];
            }

            if (val != null)
            {
                return Engine.CreateValue(val);
            }

            return null;
        }

        private InternalHandle XHRSend(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            string data = null;

            try
            {
                if (_req.Method == "POST" && args.Length == 1)
                {
                    if (args[0].IsString)
                    {
                        var pd = Encoding.UTF8.GetBytes(args[0].AsString);
                        _req.GetRequestStream().Write(pd, 0, pd.Length);
                    }
                    else if (args[0].IsArray)
                    {
                        var pd = (byte[])Types.ChangeType(args[0].Value, typeof(char[]));
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
            catch { }

            if (data != null)
            {
                return Engine.CreateValue(data);
            }

            return null;
        }

        private InternalHandle XHRAddHeader(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (args.Length == 2)
            {
                if (_req != null)
                {
                    var name = args[0].AsString;
                    var val = args[1].AsString;

                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(val))
                    {
                        if (name.ToLower() == "user-agent")
                        {
                            _req.UserAgent = val;
                        }
                        else if (name.ToLower() == "connection")
                        {
                            _req.KeepAlive = val.ToLower() == "keep-alive";
                        }
                        else
                        {
                            _req.Headers.Add(name, val);
                        }
                    }
                }
            }

            return null;
        }

        private InternalHandle XHROpen(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (args.Length == 2)
            {
                var method = args[0].AsString;
                var url = args[1].AsString;

                _req = (HttpWebRequest)WebRequest.Create(args[1].AsString);
                _req.Method = method;
            }

            return null;
        }
    }
}
