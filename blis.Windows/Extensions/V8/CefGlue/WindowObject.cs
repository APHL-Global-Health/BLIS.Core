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
using System;
using System.Threading.Tasks;
using Xilium.CefGlue;
#endregion

namespace blis.Windows.Extensions.V8.CefGlue
{
    public class WindowObject : CefV8Handler
    {
        #region Variables
        private CefV8Value Handler;
        private Configuration configuration;
        #endregion

        #region Constructor
        public WindowObject(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            configuration = Extension.GetConfiguration();
        }
        #endregion

        #region CreateObject
        public CefV8Value CreateObject()
        {
            Handler = CefV8Value.CreateObject();
            Handler.SetValue("setTimeout", CefV8Value.CreateFunction("setTimeout", this), CefV8PropertyAttribute.None);
            Handler.SetValue("setInterval", CefV8Value.CreateFunction("setInterval", this), CefV8PropertyAttribute.None);
            Handler.SetValue("back", CefV8Value.CreateFunction("back", this), CefV8PropertyAttribute.None);
            Handler.SetValue("copy", CefV8Value.CreateFunction("copy", this), CefV8PropertyAttribute.None);
            Handler.SetValue("cut", CefV8Value.CreateFunction("cut", this), CefV8PropertyAttribute.None);
            Handler.SetValue("delete", CefV8Value.CreateFunction("delete", this), CefV8PropertyAttribute.None);
            Handler.SetValue("find", CefV8Value.CreateFunction("find", this), CefV8PropertyAttribute.None);
            Handler.SetValue("forward", CefV8Value.CreateFunction("forward", this), CefV8PropertyAttribute.None);
            Handler.SetValue("getSource", CefV8Value.CreateFunction("getSource", this), CefV8PropertyAttribute.None);
            Handler.SetValue("getZoomLevel", CefV8Value.CreateFunction("getZoomLevel", this), CefV8PropertyAttribute.None);
            Handler.SetValue("load", CefV8Value.CreateFunction("load", this), CefV8PropertyAttribute.None);
            Handler.SetValue("loadString", CefV8Value.CreateFunction("loadString", this), CefV8PropertyAttribute.None);
            Handler.SetValue("paste", CefV8Value.CreateFunction("paste", this), CefV8PropertyAttribute.None);
            Handler.SetValue("print", CefV8Value.CreateFunction("print", this), CefV8PropertyAttribute.None);
            Handler.SetValue("redo", CefV8Value.CreateFunction("redo", this), CefV8PropertyAttribute.None);
            Handler.SetValue("refresh", CefV8Value.CreateFunction("refresh", this), CefV8PropertyAttribute.None);
            Handler.SetValue("selectAll", CefV8Value.CreateFunction("selectAll", this), CefV8PropertyAttribute.None);
            Handler.SetValue("showDevTools", CefV8Value.CreateFunction("showDevTools", this), CefV8PropertyAttribute.None);
            Handler.SetValue("stop", CefV8Value.CreateFunction("stop", this), CefV8PropertyAttribute.None);
            Handler.SetValue("undo", CefV8Value.CreateFunction("undo", this), CefV8PropertyAttribute.None);
            Handler.SetValue("viewSource", CefV8Value.CreateFunction("viewSource", this), CefV8PropertyAttribute.None);
            Handler.SetValue("shutdown", CefV8Value.CreateFunction("shutdown", this), CefV8PropertyAttribute.None);
            Handler.SetValue("printToPDF", CefV8Value.CreateFunction("printToPDF", this), CefV8PropertyAttribute.None);
            Handler.SetValue("getWindowSize", CefV8Value.CreateFunction("getwindowsize", this), CefV8PropertyAttribute.None);
            Handler.SetValue("update", CefV8Value.CreateFunction("update", this), CefV8PropertyAttribute.None);
            Handler.SetValue("checkForUpdates", CefV8Value.CreateFunction("checkforupdates", this), CefV8PropertyAttribute.None);
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
                
                if (name.ToLower().Equals("back"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            back();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else back();
                    return true;
                }
                else if (name.ToLower().Equals("copy"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            copy();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else copy();
                    return true;
                }
                else if (name.ToLower().Equals("cut"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            cut();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else cut();
                    return true;
                }
                else if (name.ToLower().Equals("delete"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            delete();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else delete();
                    return true;
                }
                else if (name.ToLower().Equals("find"))
                {
                    if (arguments.Length >= 5)
                    {
                        var identifier  = arguments[0].GetIntValue();
                        var searchText  = arguments[1].GetStringValue();
                        var forward     = arguments[2].GetBoolValue();
                        var matchCase   = arguments[3].GetBoolValue();
                        var findNext    = arguments[4].GetBoolValue();
                        var callback    = (arguments.Length == 6 ? arguments[5] : null);

                        if (callback != null && callback.IsFunction)
                        {
                            var runner = CefTaskRunner.GetForCurrentThread();
                            new Task(() =>
                            {
                                context.Enter();
                                find(identifier, searchText, forward, matchCase, findNext);
                                runner.PostTask(new CallbackTask(context, callback, null));
                                context.Exit();
                            }).Start();
                        }
                        else find(identifier, searchText, forward, matchCase, findNext); 
                    }
                    return true;
                }
                else if (name.ToLower().Equals("forward"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            forward();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else forward();
                    return true;
                }
                else if (name.ToLower().Equals("getsource"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            getSource();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else getSource();
                    return true;
                }
                else if (name.ToLower().Equals("getzoomlevel"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            runner.PostTask(new CallbackTask(context, callback, getZoomLevel()));
                            context.Exit();
                        }).Start();
                    }
                    else returnValue = CefV8Value.CreateDouble(getZoomLevel());
                    return true;
                }
                else if (name.ToLower().Equals("load"))
                {
                    if (arguments.Length >= 1)
                    {
                        var url = arguments[0].GetStringValue();
                        var callback = (arguments.Length == 2 ? arguments[1] : null);

                        if (callback != null && callback.IsFunction)
                        {
                            var runner = CefTaskRunner.GetForCurrentThread();
                            new Task(() =>
                            {
                                context.Enter();
                                load(url);
                                runner.PostTask(new CallbackTask(context, callback, null));
                                context.Exit();
                            }).Start();
                        }
                        else load(url);
                    }
                    return true;
                }
                else if (name.ToLower().Equals("loadstring"))
                {
                    if (arguments.Length >= 2)
                    {
                        var html = arguments[0].GetStringValue();
                        var url = arguments[1].GetStringValue();
                        var callback = (arguments.Length == 3 ? arguments[2] : null);

                        if (callback != null && callback.IsFunction)
                        {
                            var runner = CefTaskRunner.GetForCurrentThread();
                            new Task(() =>
                            {
                                context.Enter();
                                loadString(html, url);
                                runner.PostTask(new CallbackTask(context, callback, null));
                                context.Exit();
                            }).Start();
                        }
                        else loadString(html, url);
                    }
                    return true;
                }
                else if (name.ToLower().Equals("paste"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            paste();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else paste();
                    return true;
                }
                else if (name.ToLower().Equals("print"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            print();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else print();
                    return true;
                }
                else if (name.ToLower().Equals("redo"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            redo();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else redo();
                    return true;
                }
                else if (name.ToLower().Equals("refresh"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            refresh();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else refresh();
                    return true;
                }
                else if (name.ToLower().Equals("selectall"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            selectAll();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else selectAll();
                    return true;
                }
                else if (name.ToLower().Equals("showdevtools"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            showDevTools();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else showDevTools();
                    return true;
                }
                else if (name.ToLower().Equals("stop"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            stop();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else stop();
                    return true;
                }
                else if (name.ToLower().Equals("undo"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            undo();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else undo();
                    return true;
                }
                else if (name.ToLower().Equals("viewsource"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            viewSource();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else viewSource();
                    return true;
                }
                else if (name.ToLower().Equals("shutdown"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            shutdown();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else shutdown();
                    return true;
                }
                else if (name.ToLower().Equals("printtopdf"))
                {
                    if (arguments.Length >= 1)
                    {
                        var path = arguments[0].GetStringValue();
                        var settings = new CefPdfPrintSettings();
                        var _callback = new CefPdfClient();

                        var callback = (arguments.Length == 2 ? arguments[1] : null);
                        if (callback != null && callback.IsFunction)
                        {
                            var runner = CefTaskRunner.GetForCurrentThread();
                            new Task(() =>
                            {
                                context.Enter();
                                printToPDF(path, settings, null);
                                runner.PostTask(new CallbackTask(context, callback, _callback));
                                context.Exit();
                            }).Start();
                        }
                        else printToPDF(path, settings, _callback);
                    }
                    return true;
                }
                else if (name.ToLower().Equals("getwindowsize"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            getWindowSize();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else getWindowSize();
                    return true;
                }
                else if (name.ToLower().Equals("update"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            Update();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else Update();
                    return true;
                }
                else if (name.ToLower().Equals("checkforupdates"))
                {
                    var callback = (arguments.Length == 1 ? arguments[0] : null);
                    if (callback != null && callback.IsFunction)
                    {
                        var runner = CefTaskRunner.GetForCurrentThread();
                        new Task(() =>
                        {
                            context.Enter();
                            CheckForUpdates();
                            runner.PostTask(new CallbackTask(context, callback, null));
                            context.Exit();
                        }).Start();
                    }
                    else CheckForUpdates();
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                returnValue = CefV8Value.CreateNull();
                exception = ex.Message;
                return true;
            }

            return false;
        }
        #endregion
        
        #region Back
        public void back()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null) _browser.GoBack();
            }
        }
        #endregion

        #region Copy
        public void copy()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var _frame = _browser.GetMainFrame();
                    if (_frame != null) _frame.Copy();
                }
            }
        }
        #endregion

        #region Cut
        public void cut()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var _frame = _browser.GetMainFrame();
                    if (_frame != null) _frame.Cut();
                }
            }
        }
        #endregion

        #region Delete
        public void delete()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var _frame = _browser.GetMainFrame();
                    if (_frame != null) _frame.Delete();
                }
            }
        }
        #endregion

        #region Find
        public void find(int identifier, string searchText, bool forward, bool matchCase, bool findNext)
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var _host = _browser.GetHost();
                    if (_host != null) _host.Find(identifier, searchText, forward, matchCase, findNext);
                }
            }
        }
        #endregion

        #region Forward
        public void forward()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null) _browser.GoForward();
            }
        }
        #endregion

        #region GetSource
        public void getSource()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var _frame = _browser.GetMainFrame();
                    //if (_frame != null) _frame.GetSource();
                }
            }
        }
        #endregion

        #region GetZoomLevel
        public double getZoomLevel()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var _host = _browser.GetHost();
                    if (_host != null) _host.GetZoomLevel();
                }
            }
            return 0;
        }
        #endregion

        #region Load
        public void load(string url)
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var _frame = _browser.GetMainFrame();
                    if (_frame != null) _frame.LoadUrl(url);
                }
            }
        }
        #endregion

        #region LoadString
        public void loadString(string html, string url)
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var _frame = _browser.GetMainFrame();
                    if (_frame != null) _frame.LoadString(html, url);
                }
            }
        }
        #endregion

        #region Paste
        public void paste()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var _frame = _browser.GetMainFrame();
                    if (_frame != null) _frame.Paste();
                }
            }
        }
        #endregion

        #region Print
        public void print()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var _host = _browser.GetHost();
                    if (_host != null) _host.Print();
                }
            }
        }
        #endregion

        #region Redo
        public void redo()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var _frame = _browser.GetMainFrame();
                    if (_frame != null) _frame.Redo();
                }
            }
        }
        #endregion

        #region Refresh
        public void refresh()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null) _browser.Reload();
            }
        }
        #endregion

        #region SelectAll
        public void selectAll()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var _frame = _browser.GetMainFrame();
                    if (_frame != null) _frame.SelectAll();
                }
            }
        }
        #endregion

        #region ShowDevTools
        public void showDevTools()
        {
            //if (ConfigurationHelper.Scheme.Owner != null && ConfigurationHelper.Scheme.Owner.CurrentBrowser != null)
            //{
            //    var host = ConfigurationHelper.Scheme.Owner.CurrentBrowser.GetHost();
            //    var wi = CefWindowInfo.Create();
            //    var parentWindowHandle = NativeMethods.gdk_x11_drawable_get_xid(IntPtr.Zero);
            //    wi.SetAsChild(parentWindowHandle, new CefRectangle(0, 0, 0, 0));
            //    host.ShowDevTools(wi, new blis.Common.PopupWebClient(), new CefBrowserSettings(), new CefPoint(0, 0));
            //}
        }
        #endregion

        #region Stop
        public void stop()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null) _browser.StopLoad();
            }
        }
        #endregion

        #region Undo
        public void undo()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var _frame = _browser.GetMainFrame();
                    if (_frame != null) _frame.Undo();
                }
            }
        }
        #endregion

        #region ViewSource
        public void viewSource()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var _frame = _browser.GetMainFrame();
                    if (_frame != null) _frame.ViewSource();
                }
            }
        }
        #endregion

        #region Shutdown
        public void shutdown()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var message = CefProcessMessage.Create("shutdown");
                    var handled = _browser.SendProcessMessage(CefProcessId.Browser, message);
                }
            }
        }
        #endregion

        #region PrintToPDF
        public void printToPDF(string path, CefPdfPrintSettings settings, CefPdfPrintCallback callback)
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var _host = _browser.GetHost();
                    if (_host != null) _host.PrintToPdf(path, settings, callback);
                }
            }
        }
        #endregion

        #region GetWindowSize
        public void getWindowSize()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var message = CefProcessMessage.Create("getwindowsize");
                    var handled = _browser.SendProcessMessage(CefProcessId.Browser, message);
                }
            }
        }
        #endregion

        #region Update
        public void Update()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var message = CefProcessMessage.Create("update");
                    var handled = _browser.SendProcessMessage(CefProcessId.Browser, message);
                }
            }
        }
        #endregion

        #region CheckForUpdates
        public void CheckForUpdates()
        {
            var _context = CefV8Context.GetCurrentContext();
            if (_context != null)
            {
                var _browser = _context.GetBrowser();
                if (_browser != null)
                {
                    var message = CefProcessMessage.Create("checkforupdates");
                    var handled = _browser.SendProcessMessage(CefProcessId.Browser, message);
                }
            }
        }
        #endregion
    }
}
