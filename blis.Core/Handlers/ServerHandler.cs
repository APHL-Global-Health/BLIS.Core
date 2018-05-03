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
using blis.Core.Extensions.V8.CefGlue.Events;
using blis.Core.Helpers;
using blis.Core.Infrastructure;
using LitJson;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xilium.CefGlue;
#endregion

namespace blis.Core.Handlers
{
    public class ServerHandler : CefServerHandler
    {
        #region Events
        public event EventHandler<OnClientConnectedEventArgs> onClientConnected;
        public event EventHandler<OnClientDisconnectedEventArgs> onClientDisconnected;
        public event EventHandler<OnHttpRequestEventArgs> onHttpRequest;
        public event EventHandler<OnServerCreatedEventArgs> onServerCreated;
        public event EventHandler<OnServerDestroyedEventArgs> onServerDestroyed;
        public event EventHandler<OnWebSocketConnectedEventArgs> onWebSocketConnected;
        public event EventHandler<OnWebSocketMessageEventArgs> onWebSocketMessage;
        public event EventHandler<OnWebSocketRequestEventArgs> onWebSocketRequest;
        #endregion

        #region Properties
        public string Address { get; set; }
        public int Port { get; set; }
        public int Threads { get; set; }       
        public bool IsConnected { get; set; }
        #endregion

        #region Variables
        private Dictionary<string, WebCallback> webCallbacks;
        private Dictionary<int, RequestHelper> webResponseCallbacks;
        #endregion

        #region Constructor
        public ServerHandler()
        {
            this.Address = "";
            this.Port = 0;
            this.Threads = 1;

            this.webResponseCallbacks = new Dictionary<int, RequestHelper>();
            this.webCallbacks = new Dictionary<string, WebCallback>();
            this.webCallbacks.Add("onClientConnected", null);
            this.webCallbacks.Add("onClientDisconnected", null);
            this.webCallbacks.Add("onHttpRequest", null);
            this.webCallbacks.Add("onServerCreated", null);
            this.webCallbacks.Add("onServerDestroyed", null);
            this.webCallbacks.Add("onWebSocketConnected", null);
            this.webCallbacks.Add("onWebSocketMessage", null);
            this.webCallbacks.Add("onWebSocketRequest", null);

        }

        public ServerHandler(string Address, int Port, int Threads = 1)
            :this()
        {
            this.Address = Address;
            this.Port = Port;
            this.Threads = Threads;
        }
        #endregion

        #region Connect
        public void Connect()
        {
            if (!IsConnected && !string.IsNullOrEmpty(this.Address) && this.Port > 0 && this.Threads > 0)
            {
                CefServer.Create(this.Address, (ushort)this.Port, this.Threads, this);
                IsConnected = true;
            }
        }
        #endregion

        #region OnClientConnected
        protected override void OnClientConnected(CefServer server, int connectionId)
        {
            if (this.webCallbacks["onClientConnected"] != null)
            {
                try
                {
                    var _callback = this.webCallbacks["onClientConnected"];
                    new Task(() =>
                    {
                        _callback.Context.Enter();
                        _callback.Runner.PostTask(new CallbackTask(_callback.Context, _callback.Callback, connectionId));
                        _callback.Context.Exit();
                    }).Start();
                }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
            }
            if (onClientConnected != null) onClientConnected(this, new OnClientConnectedEventArgs(server, connectionId));
        }
        #endregion

        #region OnClientDisconnected
        protected override void OnClientDisconnected(CefServer server, int connectionId)
        {
            if (this.webCallbacks["onClientDisconnected"] != null)
            {
                try
                {
                    var _callback = this.webCallbacks["onClientDisconnected"];
                    new Task(() =>
                    {
                        _callback.Context.Enter();
                        _callback.Runner.PostTask(new CallbackTask(_callback.Context, _callback.Callback, connectionId));                        
                        _callback.Context.Exit();
                    }).Start();
                }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
            }
            if (onClientDisconnected != null) onClientDisconnected(this, new OnClientDisconnectedEventArgs(server, connectionId));
        }
        #endregion

        #region OnHttpRequest
        protected override void OnHttpRequest(CefServer server, int connectionId, string clientAddress, CefRequest request)
        {
            if (this.webCallbacks["onHttpRequest"] != null)
            {
                try
                {
                    var _callback = this.webCallbacks["onHttpRequest"];
                    new Task(() =>
                    {
                        _callback.Context.Enter();
                        var _request = new RequestHelper(server, request);
                        this.webResponseCallbacks.Add(connectionId, _request);
                        _callback.Runner.PostTask(new CallbackTask(_callback.Context, _callback.Callback, connectionId, clientAddress, _request.ToJson()));
                        _callback.Context.Exit();
                    }).Start();
                }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
            }
            if (onHttpRequest != null) onHttpRequest(this, new OnHttpRequestEventArgs(server, connectionId, clientAddress, request));
        }
        #endregion

        #region OnServerCreated
        protected override void OnServerCreated(CefServer server)
        {
            if (this.webCallbacks["onServerCreated"] != null)
            {
                try
                {
                    var _callback = this.webCallbacks["onServerCreated"];
                    new Task(() =>
                    {
                        _callback.Context.Enter();
                        _callback.Runner.PostTask(new CallbackTask(_callback.Context, _callback.Callback));
                        _callback.Context.Exit();
                    }).Start();
                }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
            }
            if (onServerCreated != null) onServerCreated(this, new OnServerCreatedEventArgs(server));
        }
        #endregion

        #region OnServerDestroyed
        protected override void OnServerDestroyed(CefServer server)
        {
            if (this.webCallbacks["onServerDestroyed"] != null)
            {
                try
                {
                    var _callback = this.webCallbacks["onServerDestroyed"];
                    new Task(() =>
                    {
                        _callback.Context.Enter();
                        _callback.Runner.PostTask(new CallbackTask(_callback.Context, _callback.Callback));
                        _callback.Context.Exit();
                    }).Start();
                }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
            }
            if (onServerDestroyed != null) onServerDestroyed(this, new OnServerDestroyedEventArgs(server));
        }
        #endregion

        #region OnWebSocketConnected
        protected override void OnWebSocketConnected(CefServer server, int connectionId)
        {
            if (this.webCallbacks["onWebSocketConnected"] != null)
            {
                try
                {
                    var _callback = this.webCallbacks["onWebSocketConnected"];
                    new Task(() =>
                    {
                        _callback.Context.Enter();
                        _callback.Runner.PostTask(new CallbackTask(_callback.Context, _callback.Callback, connectionId));
                        _callback.Context.Exit();
                    }).Start();
                }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
            }
            if (onWebSocketConnected != null) onWebSocketConnected(this, new OnWebSocketConnectedEventArgs(server, connectionId));
        }
        #endregion

        #region OnWebSocketMessage
        protected override void OnWebSocketMessage(CefServer server, int connectionId, IntPtr data, long dataSize)
        {
            if (this.webCallbacks["onWebSocketMessage"] != null)
            {
                try
                {
                    if (dataSize > 0)
                    {
                        byte[] messageBytes = new byte[dataSize];
                        Marshal.Copy(data, messageBytes, 0, (int)dataSize);
                        var message = Encoding.Default.GetString(messageBytes);

                        var _callback = this.webCallbacks["onWebSocketMessage"];
                        new Task(() =>
                        {
                            _callback.Context.Enter();
                            _callback.Runner.PostTask(new CallbackTask(_callback.Context, _callback.Callback, connectionId, message));
                            _callback.Context.Exit();
                        }).Start();
                    }                    
                }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
            }

            if (onWebSocketMessage != null) onWebSocketMessage(this, new OnWebSocketMessageEventArgs(server, connectionId, data, dataSize));
        }
        #endregion

        #region OnWebSocketRequest
        protected override void OnWebSocketRequest(CefServer server, int connectionId, string clientAddress, CefRequest request, CefCallback callback)
        {
            if (this.webCallbacks["onWebSocketRequest"] != null)
            {
                try
                {
                    var _callback = this.webCallbacks["onWebSocketRequest"];
                    new Task(() =>
                    {
                        _callback.Context.Enter();
                        var _request = new RequestHelper(server, request);
                        this.webResponseCallbacks.Add(connectionId, _request);                        
                        _callback.Runner.PostTask(new CallbackTask(_callback.Context, _callback.Callback, connectionId, clientAddress, _request.ToJson()));
                        _callback.Context.Exit();
                    }).Start();

                    callback.Continue();
                }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
            }
            if (onWebSocketRequest != null) onWebSocketRequest(this, new OnWebSocketRequestEventArgs(server, connectionId, clientAddress, request, callback));
        }
        #endregion

        #region AddEventListener
        public void AddEventListener(string name, WebCallback callback)
        {
            if(this.webCallbacks.Any(s=>s.Key == name))
            {
                this.webCallbacks[name] = callback;
            }
        }
        #endregion

        #region sendHttp200Response
        public void sendHttp200Response(int id, string contentType, byte[] data)
        {
            if (id > 0 && data != null && webResponseCallbacks.Keys.Contains(id))
            {
                var callback = webResponseCallbacks[id];
                if (callback != null)
                {
                    var server = callback.GetServer();
                    if (server != null)
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(data.Length);

                        try
                        {
                            Marshal.Copy(data, 0, ptr, data.Length);
                            server.SendHttp200Response(id, contentType, ptr, data.Length);
                            if (webResponseCallbacks.Keys.Contains(id)) webResponseCallbacks.Remove(id);
                        }
                        finally { Marshal.FreeHGlobal(ptr); }
                    }
                }
            }
        }
        #endregion

        #region sendHttp404Response
        public void sendHttp404Response(int id)
        {
            if (id > 0 && webResponseCallbacks.Keys.Contains(id))
            {
                var callback = webResponseCallbacks[id];
                if (callback != null)
                {
                    var server = callback.GetServer();
                    if (server != null)
                    {
                        server.SendHttp404Response(id);
                        if (webResponseCallbacks.Keys.Contains(id)) webResponseCallbacks.Remove(id);
                    }
                }
            }
        }
        #endregion

        #region sendHttp500Response
        public void sendHttp500Response(int id, string error)
        {
            if (id > 0 && webResponseCallbacks.Keys.Contains(id))
            {
                var callback = webResponseCallbacks[id];
                if (callback != null)
                {
                    var server = callback.GetServer();
                    if (server != null)
                    {
                        server.SendHttp500Response(id, error);
                        if (webResponseCallbacks.Keys.Contains(id)) webResponseCallbacks.Remove(id);
                    }
                        
                }
            }
        }
        #endregion

        #region sendHttpResponse
        public void sendHttpResponse(int id, int responseCode, string contentType, long contentLength, string headers)
        {
            if (id > 0 && webResponseCallbacks.Keys.Contains(id))
            {
                var callback = webResponseCallbacks[id];
                if (callback != null)
                {
                    var server = callback.GetServer();
                    if (server != null)
                    {                        
                        var extraHeaders = new NameValueCollection();
                        if (!string.IsNullOrEmpty(headers))
                        {
                            foreach(JsonData header in JsonMapper.ToObject(headers))
                            {
                                foreach (var key in header.Keys)
                                {
                                    var value = header[key].ToString();
                                    extraHeaders.Add(key, value);
                                }
                            }
                        }
                        
                        server.SendHttpResponse(id, responseCode, contentType, contentLength, extraHeaders);
                        if (webResponseCallbacks.Keys.Contains(id)) webResponseCallbacks.Remove(id);
                    }
                }
            }
        }
        #endregion

        #region sendRawData
        public void sendRawData(int id, byte[] data)
        {
            if (id > 0 && data != null && webResponseCallbacks.Keys.Contains(id))
            {
                var callback = webResponseCallbacks[id];
                if (callback != null)
                {
                    var server = callback.GetServer();
                    if (server != null)
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(data.Length);

                        try
                        {
                            Marshal.Copy(data, 0, ptr, data.Length);
                            server.SendRawData(id, ptr, data.Length);
                            if (webResponseCallbacks.Keys.Contains(id)) webResponseCallbacks.Remove(id);
                        }
                        finally { Marshal.FreeHGlobal(ptr); }
                    }
                }
            }
        }
        #endregion

        #region sendWebSocketMessage
        public void sendWebSocketMessage(int id, byte[] data)
        {
            if (id > 0 && data != null && webResponseCallbacks.Keys.Contains(id))
            {
                var callback = webResponseCallbacks[id];
                if (callback != null)
                {
                    var server = callback.GetServer();
                    if (server != null)
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(data.Length);

                        try
                        {
                            Marshal.Copy(data, 0, ptr, data.Length);
                            server.SendWebSocketMessage(id, ptr, data.Length);
                            if (webResponseCallbacks.Keys.Contains(id)) webResponseCallbacks.Remove(id);
                        }
                        finally { Marshal.FreeHGlobal(ptr); }
                    }
                }
            }
        }
        #endregion
    }
}
