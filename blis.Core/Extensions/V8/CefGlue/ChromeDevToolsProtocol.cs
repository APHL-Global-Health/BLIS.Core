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

using blis.Core.Configurations;
using blis.Core.Helpers;
using blis.Core.Infrastructure;
using LitJson;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using Xilium.CefGlue;
namespace blis.Core.Extensions.V8.CefGlue
{
    public class ChromeDevToolsProtocol : CefV8Handler
    {
        #region Variables
        private JsonData protocol = null;
        private Configuration configuration;
        private ObservableCollection<DevToolsSocket> DevToolsSockets { get; set; }

        #endregion

        #region Constructor
        public ChromeDevToolsProtocol(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            try
            {
                configuration = Extension.GetConfiguration();
                var _req = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:" + configuration.RemoteDebuggingPort + "/json/protocol");
                var _rsp = (HttpWebResponse)_req.GetResponse();
                if (_rsp.StatusCode == HttpStatusCode.OK)
                {
                    var jsProtocolJson = new StreamReader(_rsp.GetResponseStream()).ReadToEnd();
                    if (!string.IsNullOrEmpty(jsProtocolJson))
                    {
                        protocol = JsonMapper.ToObject(jsProtocolJson);

                        _req = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:" + configuration.RemoteDebuggingPort + "/json");
                        _rsp = (HttpWebResponse)_req.GetResponse();
                        if (_rsp.StatusCode == HttpStatusCode.OK)
                        {
                            var jsJson = new StreamReader(_rsp.GetResponseStream()).ReadToEnd();
                            if (!string.IsNullOrEmpty(jsJson))
                            {
                                var settings = JsonMapper.ToObject(jsJson);
                                this.DevToolsSockets = new ObservableCollection<DevToolsSocket>();
                                foreach (JsonData setting in settings)
                                {
                                    if (frame.Url == (string)setting["url"])// && frame.Url != "chrome-devtools://devtools/inspector.html"
                                        this.DevToolsSockets.Add(new DevToolsSocket(frame.Url, (string)setting["webSocketDebuggerUrl"], this.DevToolsSockets));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
        }
        #endregion

        #region CreateObject
        public CefV8Value CreateObject()
        {
            CefV8Value Handler = CefV8Value.CreateObject();
            Handler.SetValue("Protocols", CefV8Value.CreateFunction("Protocols", this), CefV8PropertyAttribute.None);
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
                var runner = CefTaskRunner.GetForCurrentThread();
                var browser = context.GetBrowser();
                var frame = browser.GetMainFrame();

                if (name == "Protocols") // && arguments.Length == 1
                {
                    //CefV8Value callback = arguments[0];
                    var Handler = CefV8Value.CreateObject();

                    //if (callback.IsFunction)
                    {
                        if (protocol != null && protocol.Keys.Contains("domains"))
                        {
                            foreach (JsonData _domain in protocol["domains"])
                            {
                                if (_domain.Keys.Contains("domain"))
                                {
                                    var DomainHandler = CefV8Value.CreateObject();

                                    var domainName = _domain["domain"].ToString();

                                    if (_domain.Keys.Contains("commands"))
                                    {
                                        foreach (JsonData command in _domain["commands"])
                                        {
                                            var commandName = command["name"].ToString();
                                            DomainHandler.SetValue(commandName, CefV8Value.CreateFunction(domainName + "." + commandName, this), CefV8PropertyAttribute.None);
                                        }
                                    }

                                    if (_domain.Keys.Contains("events"))
                                    {
                                        foreach (JsonData @event in _domain["events"])
                                        {
                                            var eventName = @event["name"].ToString();
                                            DomainHandler.SetValue(eventName, CefV8Value.CreateFunction(domainName + "." + eventName, this), CefV8PropertyAttribute.None);
                                        }
                                    }

                                    if (_domain.Keys.Contains("types"))
                                    {
                                        foreach (JsonData @type in _domain["types"])
                                        {
                                            var typeName = @type["id"].ToString();
                                            DomainHandler.SetValue(typeName, CefV8Value.CreateFunction(domainName + "." + typeName, this), CefV8PropertyAttribute.None);
                                        }
                                    }

                                    Handler.SetValue(domainName, DomainHandler, CefV8PropertyAttribute.None);
                                }
                            }
                        }

                        //new Task(() =>
                        //{
                        //    context.Enter();
                        //    runner.PostTask(new CallbackTask(context, callback));
                        //    context.Exit();
                        //}).Start();

                    }
                    returnValue = Handler;
                }
                else
                {
                    if (frame != null)
                    {
                        var sock = this.DevToolsSockets.FirstOrDefault(s => s.Url == frame.Url);
                        if (sock != null)
                        {
                            string @params = "{}";
                            CefV8Value callback = null;
                            if (arguments.Length >= 1)
                            {
                                if (arguments[0].IsFunction) callback = arguments[0];
                                else
                                {
                                    @params = Extension.CefV8ValueToJson(arguments[0]);
                                    if (arguments.Length >= 2 && arguments[1].IsFunction) callback = arguments[1];
                                }
                            }

                            WebCallback webSocketCallback = null;
                            if (callback != null && callback.IsFunction)
                                webSocketCallback = new WebCallback(context, runner, callback);

                            sock.Query(name, @params, webSocketCallback);

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
    }
}
