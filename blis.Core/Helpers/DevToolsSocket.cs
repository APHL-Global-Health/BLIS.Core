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
using LitJson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WebSocketSharp;

namespace blis.Core.Helpers
{
    public class DevToolsSocket
    {
        #region Variables
        private WebSocket webSocket { get; set; }
        internal Dictionary<int, WebCallback> webSocketCallbacks;
        internal int webSocketId;
        #endregion

        #region Properties
        public string Url { get; set; }
        public string WebSocketDebuggerUrl { get; set; }
        #endregion

        #region Constructor
        public DevToolsSocket(string url, string webSocketDebuggerUrl, ObservableCollection<DevToolsSocket> DevToolsSockets)
        {
            this.Url = url;
            this.webSocketId = 0;
            this.WebSocketDebuggerUrl = webSocketDebuggerUrl;
            this.webSocketCallbacks = new Dictionary<int, WebCallback>();

            if (!string.IsNullOrEmpty(webSocketDebuggerUrl))
            {
                this.webSocket = new WebSocket(webSocketDebuggerUrl);
                this.webSocket.OnOpen += (sender, e) =>
                {
                    Console.WriteLine();
                };

                this.webSocket.OnMessage += (sender, e) =>
                {
                    var jsDataJson = JsonMapper.ToObject(e.Data);
                    if (jsDataJson.Keys.Contains("id"))
                    {
                        var id = (int)jsDataJson["id"];
                        if (this.webSocketCallbacks.ContainsKey(id))
                        {
                            try
                            {
                                var _callback = this.webSocketCallbacks[id];
                                new Task(() =>
                                {
                                    _callback.Context.Enter();
                                    _callback.Runner.PostTask(new CallbackTask(_callback.Context, _callback.Callback, "", e.Data));
                                    this.webSocketCallbacks.Remove(id);
                                    _callback.Context.Exit();
                                }).Start();
                            }
                            catch (Exception ex)
                            {
                                Log.Trace(ex.Message, ex.StackTrace, true);
                                this.webSocketCallbacks.Remove(id);
                            }
                        }
                    }
                };

                this.webSocket.OnError += (sender, ex) =>
                {
                    Log.Trace(ex.Message, true);
                    Console.WriteLine(ex.Message);
                };

                this.webSocket.OnClose += (sender, e) =>
                {
                    if(DevToolsSockets != null && DevToolsSockets.Any(s=>s.Url == this.Url && s.WebSocketDebuggerUrl == this.WebSocketDebuggerUrl)) DevToolsSockets.Remove(this);
                };
                Connect();
            }
        }
        #endregion

        public void Connect()
        {
            if(this.webSocket != null) this.webSocket.Connect();
        }

        public void Close()
        {
            if (this.webSocket != null) this.webSocket.Close();
        }

        public bool IsConnected()
        {
            if (this.webSocket != null) return this.webSocket.IsAlive;
            return false;
        }

        public void Query(string method, string @params, WebCallback callback)
        {
            this.webSocketId++;

            if (callback != null && this.webSocketCallbacks != null) this.webSocketCallbacks.Add(webSocketId, callback);

            var query = "{\"id\": " + (this.webSocketId) + ", \"method\": \"" + method + "\", \"params\": " + @params + "}";
            if (this.webSocket != null) this.webSocket.Send(query);
        }
    }
}
