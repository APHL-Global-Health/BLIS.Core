// --------------------------------------------------------------------------------------------------------------------
///<license>
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

namespace blis.Core.Browser.Handlers
{
    using blis.Core.Extensions.V8.CefGlue;
    using blis.Core.Handlers;
    using blis.Windows.Extensions.V8.CefGlue;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xilium.CefGlue;
    using Xilium.CefGlue.Wrapper;

    /// <summary>
    /// The CefGlue render process handler.
    /// </summary>
    public class CefGlueRenderProcessHandler : CefRenderProcessHandler
    {
        #region Properties
        /// <summary>
        /// Gets the message router.
        /// </summary>
        internal CefMessageRouterRendererSide MessageRouter { get; }
        internal ObservableCollection<ServerHandler> Servers { get; set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueRenderProcessHandler"/> class.
        /// </summary>
        public CefGlueRenderProcessHandler()
        {
            this.MessageRouter = new CefMessageRouterRendererSide(new CefMessageRouterConfig());
            this.Servers = new ObservableCollection<ServerHandler>();
        }
        
        /// <summary>
        /// The on context created.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected override void OnContextCreated(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            #region Initialize
            var global = context.GetGlobal();
            #endregion

            #region Exposed
            CefV8Value exposed = CefV8Value.CreateObject();
            global.SetValue("exposed", exposed, CefV8PropertyAttribute.None);
            #endregion

            #region Cef Assembly
            var extension = new CefAssembly(browser, frame, context);            
            extension.CreateObject(global, new List<string>() { "mscorlib", "System", "System.Core"});
            //extension.CreateObject(global, AppDomain.CurrentDomain.GetAssemblies().Where(s =>
            //{
            //    if (s.GetName().Name.StartsWith("blis")) return true;
            //    return false;
            //})
            //.Select(s => s.GetName().Name).ToList());
            #endregion

            #region Custom XHR
            var xhr = new xhrObject(browser, frame, context);
            global.SetValue("xHttpRequest", xhr.CreateObject(), CefV8PropertyAttribute.None);
            #endregion

            #region V8 Engine API
            var v8Engine = new V8EngineObject(browser, frame, context);
            global.SetValue("V8Engine", v8Engine.CreateObject(), CefV8PropertyAttribute.None);
            #endregion

            #region Database API
            var database = new DatabaseObject(browser, frame, context);
            global.SetValue("DB", database.CreateObject(), CefV8PropertyAttribute.None);
            #endregion

            #region Console API
            var console = new ConsoleObject(browser, frame, context);
            global.SetValue("Console", database.CreateObject(), CefV8PropertyAttribute.None);
            #endregion

            #region Window API
            var window = new WindowObject(browser, frame, context);
            global.SetValue("Window", window.CreateObject(), CefV8PropertyAttribute.None);
            #endregion
            
            #region Server API
            var server = new ServerObject(browser, frame, context, this);
            global.SetValue("Server", server.CreateObject(), CefV8PropertyAttribute.None);
            #endregion

            #region ChromeDevToosProtocol Object
            var chromeDevToolsProtocol = new ChromeDevToolsProtocol(browser, frame, context);
            global.SetValue("DevTools", chromeDevToolsProtocol.CreateObject(), CefV8PropertyAttribute.None);
            #endregion

            #region V8 Object
            //var v8object = new V8Object(browser, frame, context);
            global.SetValue("V8Object", v8Engine.v8object.CreateObject(), CefV8PropertyAttribute.None);
            #endregion

            #region OnContextCreated
            MessageRouter.OnContextCreated(browser, frame, context);
            context.Dispose();
            #endregion
        }

        /// <summary>
        /// The on context released.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected override void OnContextReleased(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            // MessageRouter.OnContextReleased releases captured CefV8Context (if have).
            this.MessageRouter.OnContextReleased(browser, frame, context);

            // Release CefV8Context.
            context.Dispose();
        }

        /// <summary>
        /// The on process message received.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="sourceProcess">
        /// The source process.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        {
            var handled = this.MessageRouter.OnProcessMessageReceived(browser, sourceProcess, message);
            if (handled)
            {
                return true;
            }

            return false;
        }

        protected override CefLoadHandler GetLoadHandler()
        {
            return base.GetLoadHandler();
        }

        //protected override bool OnBeforeNavigation(CefBrowser browser, CefFrame frame, CefRequest request, CefNavigationType navigation_type, bool isRedirect)
        //{
        //    return base.OnBeforeNavigation(browser, frame, request, navigation_type, isRedirect);
        //}

        protected override void OnBrowserDestroyed(CefBrowser browser)
        {
            base.OnBrowserDestroyed(browser);
        }

        protected override void OnBrowserCreated(CefBrowser browser)
        {
            base.OnBrowserCreated(browser);
        }

        protected override void OnFocusedNodeChanged(CefBrowser browser, CefFrame frame, CefDomNode node)
        {
            base.OnFocusedNodeChanged(browser, frame, node);
        }

        protected override void OnRenderThreadCreated(CefListValue extraInfo)
        {
            base.OnRenderThreadCreated(extraInfo);
        }

        protected override void OnUncaughtException(CefBrowser browser, CefFrame frame, CefV8Context context, CefV8Exception exception, CefV8StackTrace stackTrace)
        {
            base.OnUncaughtException(browser, frame, context, exception, stackTrace);
        }

        protected override void OnWebKitInitialized()
        {
            base.OnWebKitInitialized();
        }
    }
}
