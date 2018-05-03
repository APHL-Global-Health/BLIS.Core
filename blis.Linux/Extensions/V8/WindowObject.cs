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
using System.Threading.Tasks;
using V8.Net;
using Xilium.CefGlue;
#endregion

namespace blis.Linux.Extensions.V8
{
    public class WindowObject : V8NativeObject
    {
        #region Variables
        private Configuration configuration;
        private CefBrowser browser;
        #endregion

        #region AddBrowser
        public void AddBrowser(CefBrowser browser)
        {
            this.browser = browser;
        }
        #endregion

        #region Initialize
        public override InternalHandle Initialize(bool isConstructCall, params InternalHandle[] args)
        {
            configuration = Extension.GetConfiguration();

            this.AsDynamic.setTimeout = Engine.CreateFunctionTemplate().GetFunctionObject(setTimeout);
            this.AsDynamic.setInterval = Engine.CreateFunctionTemplate().GetFunctionObject(setInterval);
            this.AsDynamic.back = Engine.CreateFunctionTemplate().GetFunctionObject(back);
            this.AsDynamic.copy = Engine.CreateFunctionTemplate().GetFunctionObject(copy);
            this.AsDynamic.cut = Engine.CreateFunctionTemplate().GetFunctionObject(cut);
            this.AsDynamic.delete = Engine.CreateFunctionTemplate().GetFunctionObject(delete);
            this.AsDynamic.find = Engine.CreateFunctionTemplate().GetFunctionObject(find);
            this.AsDynamic.forward = Engine.CreateFunctionTemplate().GetFunctionObject(forward);
            this.AsDynamic.getSource = Engine.CreateFunctionTemplate().GetFunctionObject(getSource);
            this.AsDynamic.getZoomLevel = Engine.CreateFunctionTemplate().GetFunctionObject(getZoomLevel);
            this.AsDynamic.load = Engine.CreateFunctionTemplate().GetFunctionObject(load);
            this.AsDynamic.loadString = Engine.CreateFunctionTemplate().GetFunctionObject(loadString);
            this.AsDynamic.paste = Engine.CreateFunctionTemplate().GetFunctionObject(paste);
            this.AsDynamic.print = Engine.CreateFunctionTemplate().GetFunctionObject(print);
            this.AsDynamic.redo = Engine.CreateFunctionTemplate().GetFunctionObject(redo);
            this.AsDynamic.refresh = Engine.CreateFunctionTemplate().GetFunctionObject(refresh);
            this.AsDynamic.selectAll = Engine.CreateFunctionTemplate().GetFunctionObject(selectAll);
            this.AsDynamic.showDevTools = Engine.CreateFunctionTemplate().GetFunctionObject(showDevTools);
            this.AsDynamic.stop = Engine.CreateFunctionTemplate().GetFunctionObject(stop);
            this.AsDynamic.undo = Engine.CreateFunctionTemplate().GetFunctionObject(undo);
            this.AsDynamic.viewSource = Engine.CreateFunctionTemplate().GetFunctionObject(viewSource);
            this.AsDynamic.shutdown = Engine.CreateFunctionTemplate().GetFunctionObject(shutdown);
            this.AsDynamic.printToPDF = Engine.CreateFunctionTemplate().GetFunctionObject(printToPDF);
            this.AsDynamic.getWindowSize = Engine.CreateFunctionTemplate().GetFunctionObject(getWindowSize);
            this.AsDynamic.Update = Engine.CreateFunctionTemplate().GetFunctionObject(Update);
            this.AsDynamic.CheckForUpdates = Engine.CreateFunctionTemplate().GetFunctionObject(CheckForUpdates);

            this.AsDynamic.executeFunction = Engine.CreateFunctionTemplate().GetFunctionObject(executeFunction);
            this.AsDynamic.executeJavaScript = Engine.CreateFunctionTemplate().GetFunctionObject(executeJavaScript);

            return base.Initialize(isConstructCall, args);
        }
        #endregion

        #region executeJavaScript
        public InternalHandle executeJavaScript(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            try
            {
                if (args.Length >= 1)
                {
                    var code    = args[0].AsString;
                    var url     = (args.Length >= 2 ? args[1].AsString : "");
                    var line    = (args.Length >= 3 ? args[2].AsInt32 : 0);

                    if (!string.IsNullOrEmpty(code))
                    {
                        var _context = CefV8Context.GetCurrentContext();
                        if (_context != null)
                        {
                            var _browser = _context.GetBrowser();
                            if (_browser != null)
                            {
                                var _frame = _browser.GetMainFrame();
                                if (_frame != null) _frame.ExecuteJavaScript(code, url, line);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose);
            }

            return Engine.CreateNullValue();
        }
        #endregion

        #region executeFunction
        public InternalHandle executeFunction(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            try
            {
                if (args.Length >= 2)
                {
                    var id = args[0].AsString;
                    if (!string.IsNullOrEmpty(id) && args[1].IsArray)
                    {
                        var _context = CefV8Context.GetCurrentContext();
                        if (_context != null)
                        {
                            var _global = _context.GetGlobal();
                            if (_global != null)
                            {
                                var _function = _global.GetValue(id);
                                if (_function != null && _function.IsFunction)
                                {
                                    var len = args[1].ArrayLength;
                                    var parameters = new dynamic[len];
                                    for (int x = 0; x < len; x++) { parameters[x] = args[1].GetProperty(x).Value; }
                                    
                                    var runner = CefTaskRunner.GetForCurrentThread();
                                    new Task(() =>
                                    {
                                        _context.Enter();
                                        if (runner != null) runner.PostTask(new CallbackTask(_context, _function, parameters));
                                        _context.Exit();
                                    }).Start();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose);
            }

            return Engine.CreateNullValue();
        }
        #endregion

        #region SetTimeout 
        private InternalHandle setTimeout(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            try
            {
                if (args.Length == 2 && args[0].IsFunction)
                {
                    var callback = args[0].KeepAlive();
                    var timeout = args[1].AsInt32;

                    var timer = new System.Timers.Timer() { Interval = timeout };
                    timer.Elapsed += (s, a) =>
                    {
                        timer.Stop();
                        try { callback.StaticCall(); }
                        catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
                    };
                    timer.Start();
                }
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
            return Engine.CreateNullValue();
        }
        #endregion

        #region SetInterval 
        private InternalHandle setInterval(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            try
            {
                if (args.Length == 2 && args[0].IsFunction)
                {
                    var callback = args[0].KeepAlive();
                    var timeout = args[1].AsInt32;

                    var timer = new System.Timers.Timer() { Interval = timeout };
                    timer.Elapsed += (s, a) =>
                    {
                    //timer.Stop();
                    try { callback.StaticCall(); }
                        catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
                    };
                    timer.Start();
                }
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }

            return Engine.CreateNullValue();
        }
        #endregion

        #region Back
        public InternalHandle back(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                browser.GoBack();
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region Copy
        public InternalHandle copy(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                browser.GetMainFrame().Copy();
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region Cut
        public InternalHandle cut(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                browser.GetMainFrame().Cut();
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region Delete
        public InternalHandle delete(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                browser.GetMainFrame().Delete();
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region Find
        public InternalHandle find(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            try
            {
                if (browser != null)
                {
                    var identifier = args[0].AsInt32;
                    var searchText = args[1].AsString;
                    var forward = args[2].AsBoolean;
                    var matchCase = args[3].AsBoolean;
                    var findNext = args[4].AsBoolean;
                    browser.GetHost().Find(identifier, searchText, forward, matchCase, findNext);
                }
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
            return Engine.CreateNullValue();
        }
        #endregion

        #region Forward
        public InternalHandle forward(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                browser.GoForward();
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region GetSource
        public InternalHandle getSource(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                //browser.GetMainFrame().GetSource();
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region GetZoomLevel
        public InternalHandle getZoomLevel(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                return Engine.CreateValue(browser.GetHost().GetZoomLevel());
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region Load
        public InternalHandle load(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (args.Length == 1 && browser != null)
            {
                var url = args[0].AsString;
                browser.GetMainFrame().LoadUrl(url);
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region LoadString
        public InternalHandle loadString(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (args.Length == 2 && browser != null)
            {
                var html = args[0].AsString;
                var url = args[1].AsString;
                browser.GetMainFrame().LoadString(html, url);
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region Paste
        public InternalHandle paste(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                browser.GetMainFrame().Paste();
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region Print
        public InternalHandle print(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                browser.GetHost().Print();
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region Redo
        public InternalHandle redo(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                browser.GetMainFrame().Redo();
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region Refresh
        public InternalHandle refresh(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                browser.Reload();
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region SelectAll
        public InternalHandle selectAll(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                browser.GetMainFrame().SelectAll();
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region ShowDevTools
        public InternalHandle showDevTools(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                //var host = browser.GetHost();
                //var wi = CefWindowInfo.Create();
                //var parentWindowHandle = NativeMethods.gdk_x11_drawable_get_xid(IntPtr.Zero);
                //wi.SetAsChild(parentWindowHandle, new CefRectangle(0, 0, 0, 0));
                //host.ShowDevTools(wi, new WebClient(null), new CefBrowserSettings(), new CefPoint(0, 0));
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region Stop
        public InternalHandle stop(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                browser.StopLoad();
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region Undo
        public InternalHandle undo(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                browser.GetMainFrame().Undo();
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region ViewSource
        public InternalHandle viewSource(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (browser != null)
            {
                browser.GetMainFrame().ViewSource();
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region Shutdown
        public InternalHandle shutdown(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            //ConfigurationHelper.Scheme.Owner.Shutdown();
            return Engine.CreateNullValue();
        }
        #endregion

        #region PrintToPDF
        public InternalHandle printToPDF(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (args.Length == 2 && browser != null)
            {
                var path = args[0].AsString;
                var settings = new CefPdfPrintSettings();
                var _callback = new CefPdfClient();
                browser.GetHost().PrintToPdf(path, settings, _callback);
            }
            return Engine.CreateNullValue();
        }
        #endregion

        #region GetWindowSize
        public InternalHandle getWindowSize(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            //if (ConfigurationHelper.Scheme.Owner != null) ConfigurationHelper.Scheme.Owner.GetWindowSize();
            return Engine.CreateNullValue();
        }
        #endregion

        #region Update
        public InternalHandle Update(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            //if (ConfigurationHelper.Scheme.Owner != null) ConfigurationHelper.Scheme.Owner.Update();
            return Engine.CreateNullValue();
        }
        #endregion

        #region CheckForUpdates
        public InternalHandle CheckForUpdates(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            //if (ConfigurationHelper.Scheme.Owner != null) ConfigurationHelper.Scheme.Owner.CheckForUpdates();
            return Engine.CreateNullValue();
        }
        #endregion
    }
}
