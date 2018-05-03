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

using blis.Core.Browser.Handlers;
using blis.Core.Handlers;
using blis.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xilium.CefGlue;

namespace blis.Windows.Extensions.V8.CefGlue
{
    public class ServerObject : CefV8Handler
    {
        #region Variables
        private CefV8Value Handler;
        private CefGlueRenderProcessHandler parent;
        #endregion

        #region Constructor
        public ServerObject(CefBrowser browser, CefFrame frame, CefV8Context context, CefGlueRenderProcessHandler parent)
        {
            this.parent = parent;            
        }
        #endregion

        #region CreateObject
        public CefV8Value CreateObject()
        {
            Handler = CefV8Value.CreateObject();
            Handler.SetValue("create", CefV8Value.CreateFunction("create", this), CefV8PropertyAttribute.None);
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
                var runner  = CefTaskRunner.GetForCurrentThread();
                var browser = context.GetBrowser();
                var frame   = browser.GetMainFrame();

                if (name == "create")
                {
                    var address = "";
                    var port = 0;
                    var threads = 1;

                    if (arguments.Length >= 1) address = arguments[0].GetStringValue();
                    if (arguments.Length >= 2) port = arguments[1].GetIntValue();
                    if (arguments.Length >= 3) threads = arguments[2].GetIntValue();
                    
                    var handler = CefV8Value.CreateObject();
                    handler.SetValue("Address", CefV8Value.CreateString(address), CefV8PropertyAttribute.None);
                    handler.SetValue("Port", CefV8Value.CreateInt(port), CefV8PropertyAttribute.None);
                    handler.SetValue("Threads", CefV8Value.CreateInt(threads), CefV8PropertyAttribute.None);

                    handler.SetValue("onClientConnected", CefV8Value.CreateFunction("onClientConnected", this), CefV8PropertyAttribute.None);
                    handler.SetValue("onClientDisconnected", CefV8Value.CreateFunction("onClientDisconnected", this), CefV8PropertyAttribute.None);
                    handler.SetValue("onHttpRequest", CefV8Value.CreateFunction("onHttpRequest", this), CefV8PropertyAttribute.None);
                    handler.SetValue("onServerCreated", CefV8Value.CreateFunction("onServerCreated", this), CefV8PropertyAttribute.None);
                    handler.SetValue("onServerDestroyed", CefV8Value.CreateFunction("onServerDestroyed", this), CefV8PropertyAttribute.None);
                    handler.SetValue("onWebSocketConnected", CefV8Value.CreateFunction("onWebSocketConnected", this), CefV8PropertyAttribute.None);
                    handler.SetValue("onWebSocketMessage", CefV8Value.CreateFunction("onWebSocketMessage", this), CefV8PropertyAttribute.None);
                    handler.SetValue("onWebSocketRequest", CefV8Value.CreateFunction("onWebSocketRequest", this), CefV8PropertyAttribute.None);
                    
                    handler.SetValue("sendHttp200Response", CefV8Value.CreateFunction("sendHttp200Response", this), CefV8PropertyAttribute.None);
                    handler.SetValue("sendHttp404Response", CefV8Value.CreateFunction("sendHttp404Response", this), CefV8PropertyAttribute.None);
                    handler.SetValue("sendHttp500Response", CefV8Value.CreateFunction("sendHttp500Response", this), CefV8PropertyAttribute.None);
                    handler.SetValue("sendHttpResponse", CefV8Value.CreateFunction("sendHttpResponse", this), CefV8PropertyAttribute.None);
                    handler.SetValue("sendRawData", CefV8Value.CreateFunction("sendRawData", this), CefV8PropertyAttribute.None);
                    handler.SetValue("sendWebSocketMessage", CefV8Value.CreateFunction("sendWebSocketMessage", this), CefV8PropertyAttribute.None);

                    handler.SetValue("connect", CefV8Value.CreateFunction("connect", this), CefV8PropertyAttribute.None);

                    if(this.parent != null && !this.parent.Servers.Any(s=>s.Address == address && s.Port == port && s.Threads == threads)) this.parent.Servers.Add(new ServerHandler(address, port, threads));
                    returnValue = handler;
                }
                else if (name == "connect")
                {
                    var dic = GetSettings(obj, arguments);
                    if (this.parent != null && dic["Address"] != null && dic["Port"] != null && dic["Threads"] != null && !string.IsNullOrEmpty(dic["Address"].ToString()) && (int)dic["Port"] > 0 && (int)dic["Threads"] > 0)
                    {
                        var serverHandler = this.parent.Servers.SingleOrDefault(s => !string.IsNullOrEmpty(s.Address) && s.Address == dic["Address"].ToString() && s.Port == (int)dic["Port"] && s.Threads == (int)dic["Threads"]);
                        if (serverHandler != null)
                        {
                            serverHandler.Connect();
                            if (arguments.Length >= 1 && arguments[0].IsFunction)
                            {
                                new Task(() =>
                                {
                                    context.Enter();
                                    runner.PostTask(new CallbackTask(context, arguments[0]));
                                    context.Exit();
                                }).Start();
                            }
                        }
                    }
                }
                else if (name == "onClientConnected" ||
                         name == "onClientDisconnected" ||
                         name == "onHttpRequest" ||
                         name == "onServerCreated" ||
                         name == "onServerDestroyed" ||
                         name == "onWebSocketConnected" ||
                         name == "onWebSocketMessage" ||
                         name == "onWebSocketRequest")
                {
                    var dic = GetSettings(obj, arguments);
                    if (this.parent != null && dic["Address"] != null && dic["Port"] != null && dic["Threads"] != null && !string.IsNullOrEmpty(dic["Address"].ToString()) && (int)dic["Port"] > 0 && (int)dic["Threads"] > 0)
                    {
                        var serverHandler = this.parent.Servers.SingleOrDefault(s => !string.IsNullOrEmpty(s.Address) && s.Address == dic["Address"].ToString() && s.Port == (int)dic["Port"] && s.Threads == (int)dic["Threads"]);
                        if (serverHandler != null && arguments.Length >= 1 && arguments[0].IsFunction)
                            serverHandler.AddEventListener(name, new WebCallback(context, runner, arguments[0]));
                    }
                }
                else if (name == "sendHttp200Response")
                {
                    var dic = GetSettings(obj, arguments);
                    if (this.parent != null && dic["Address"] != null && dic["Port"] != null && dic["Threads"] != null && !string.IsNullOrEmpty(dic["Address"].ToString()) && (int)dic["Port"] > 0 && (int)dic["Threads"] > 0)
                    {
                        var serverHandler = this.parent.Servers.SingleOrDefault(s => !string.IsNullOrEmpty(s.Address) && s.Address == dic["Address"].ToString() && s.Port == (int)dic["Port"] && s.Threads == (int)dic["Threads"]);
                        if (serverHandler != null && arguments.Length >= 3)
                        {
                            var id = arguments[0].GetIntValue();
                            var contentType = arguments[1].GetStringValue();
                            var data = Encoding.UTF8.GetBytes(arguments[2].GetStringValue());
                            serverHandler.sendHttp200Response(id, contentType, data);
                        }
                    }
                }
                else if (name == "sendHttp404Response")
                {
                    var dic = GetSettings(obj, arguments);
                    if (this.parent != null && dic["Address"] != null && dic["Port"] != null && dic["Threads"] != null && !string.IsNullOrEmpty(dic["Address"].ToString()) && (int)dic["Port"] > 0 && (int)dic["Threads"] > 0)
                    {
                        var serverHandler = this.parent.Servers.SingleOrDefault(s => !string.IsNullOrEmpty(s.Address) && s.Address == dic["Address"].ToString() && s.Port == (int)dic["Port"] && s.Threads == (int)dic["Threads"]);
                        if (serverHandler != null && arguments.Length >= 1)
                        {
                            var id = arguments[0].GetIntValue();
                            serverHandler.sendHttp404Response(id);
                        }
                    }
                }
                else if (name == "sendHttp500Response")
                {
                    var dic = GetSettings(obj, arguments);
                    if (this.parent != null && dic["Address"] != null && dic["Port"] != null && dic["Threads"] != null && !string.IsNullOrEmpty(dic["Address"].ToString()) && (int)dic["Port"] > 0 && (int)dic["Threads"] > 0)
                    {
                        var serverHandler = this.parent.Servers.SingleOrDefault(s => !string.IsNullOrEmpty(s.Address) && s.Address == dic["Address"].ToString() && s.Port == (int)dic["Port"] && s.Threads == (int)dic["Threads"]);
                        if (serverHandler != null && arguments.Length >= 2)
                        {
                            var id = arguments[0].GetIntValue();
                            var error = arguments[1].GetStringValue();
                            serverHandler.sendHttp500Response(id, error);
                        }
                    }
                }
                else if (name == "sendHttpResponse")
                {
                    var dic = GetSettings(obj, arguments);
                    if (this.parent != null && dic["Address"] != null && dic["Port"] != null && dic["Threads"] != null && !string.IsNullOrEmpty(dic["Address"].ToString()) && (int)dic["Port"] > 0 && (int)dic["Threads"] > 0)
                    {
                        var serverHandler = this.parent.Servers.SingleOrDefault(s => !string.IsNullOrEmpty(s.Address) && s.Address == dic["Address"].ToString() && s.Port == (int)dic["Port"] && s.Threads == (int)dic["Threads"]);
                        if (serverHandler != null && arguments.Length >= 5)
                        {
                            var id = arguments[0].GetIntValue();
                            var responseCode = arguments[1].GetIntValue();
                            var contentType = arguments[2].GetStringValue();
                            var contentLength = arguments[3].GetIntValue();
                            var headers = arguments[4].GetStringValue();
                            serverHandler.sendHttpResponse(id, responseCode, contentType, contentLength, headers);
                        }
                    }
                }
                else if (name == "sendRawData")
                {
                    var dic = GetSettings(obj, arguments);
                    if (this.parent != null && dic["Address"] != null && dic["Port"] != null && dic["Threads"] != null && !string.IsNullOrEmpty(dic["Address"].ToString()) && (int)dic["Port"] > 0 && (int)dic["Threads"] > 0)
                    {
                        var serverHandler = this.parent.Servers.SingleOrDefault(s => !string.IsNullOrEmpty(s.Address) && s.Address == dic["Address"].ToString() && s.Port == (int)dic["Port"] && s.Threads == (int)dic["Threads"]);
                        if (serverHandler != null && arguments.Length >= 2)
                        {
                            var id = arguments[0].GetIntValue();
                            var data = Encoding.UTF8.GetBytes(arguments[1].GetStringValue());
                            serverHandler.sendRawData(id, data);
                        }
                    }
                }
                else if (name == "sendWebSocketMessage")
                {
                    var dic = GetSettings(obj, arguments);
                    if (this.parent != null && dic["Address"] != null && dic["Port"] != null && dic["Threads"] != null && !string.IsNullOrEmpty(dic["Address"].ToString()) && (int)dic["Port"] > 0 && (int)dic["Threads"] > 0)
                    {
                        var serverHandler = this.parent.Servers.SingleOrDefault(s => !string.IsNullOrEmpty(s.Address) && s.Address == dic["Address"].ToString() && s.Port == (int)dic["Port"] && s.Threads == (int)dic["Threads"]);
                        if (serverHandler != null && arguments.Length >= 2)
                        {
                            var id = arguments[0].GetIntValue();
                            var data = Encoding.UTF8.GetBytes(arguments[1].GetStringValue());
                            serverHandler.sendWebSocketMessage(id, data);
                        }
                    }
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

        #region GetSettings
        private Dictionary<string,object> GetSettings(CefV8Value obj, CefV8Value[] arguments)
        {
            var dic = new Dictionary<string, object>();
            dic.Add("Address", "");
            dic.Add("Port", 0);
            dic.Add("Threads", 0);

            var keys = obj.GetKeys();
            var addressKey = keys.FirstOrDefault(s => s.ToLower() == "address");
            if (!string.IsNullOrEmpty(addressKey))
                dic["Address"] = obj.GetValue(addressKey).GetStringValue();

            var portKey = keys.FirstOrDefault(s => s.ToLower() == "port");
            if (!string.IsNullOrEmpty(portKey))
                dic["Port"] = obj.GetValue(portKey).GetIntValue();

            var threadsKey = keys.FirstOrDefault(s => s.ToLower() == "threads");
            if (!string.IsNullOrEmpty(threadsKey))
                dic["Threads"] = obj.GetValue(threadsKey).GetIntValue();

            return dic;
        }
        #endregion
    }
}
