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

using blis.Core;
using blis.Core.Infrastructure;
using blis.Linux.Browser;
using blis.Linux.ChromeHost;
using System;
using System.Drawing;
using Xilium.CefGlue;
using XSharp;

namespace blis.Linux
{
    public class Window : XWindowEx
	{
        /// <summary>
        /// The host/app/window application.
        /// </summary>
        private readonly HostBase mApplication;

        /// <summary>
        /// The host config.
        /// </summary>
        private readonly CefConfiguration mHostConfig;

        /// <summary>
        /// The CefGlueBrowser object.
        /// </summary>
        private readonly CefGlueBrowser mCore;

        /// <summary>
        /// The browser window handle.
        /// </summary>
        private IntPtr mBrowserWindowHandle;

        /// <summary>
        /// The web browser.
        /// </summary>
        public CefGlueBrowser WebBrowser => this.mCore;

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public Window(HostBase application, CefConfiguration hostConfig)
            : base(new XDisplay(IntPtr.Zero), new Rectangle(0, 0, application.HostConfig.HostWidth, application.HostConfig.HostHeight))
        {
            CefGlueClient.window = this;
            this.Name = application.HostConfig.HostTitle;
            
            this.mHostConfig = hostConfig;
            this.mApplication = application;
            this.PropertyNotifyHandlerEvent += (xevent, window) =>
            {
                var attr = this.GetAttributes();
                if (attr.x + attr.width > 0 && attr.y + attr.height > 0 && this.mBrowserWindowHandle != IntPtr.Zero)
                {
                   ResizeEx(this.mBrowserWindowHandle, new Size(attr.x + attr.width, attr.y + attr.height));
                }
            };

            #region Browser
            this.mCore = new CefGlueBrowser(this, new CefBrowserSettings(), this.mHostConfig.StartUrl);
            var windowInfo = CefWindowInfo.Create();
            switch (CefRuntime.Platform)
            {
                case CefRuntimePlatform.Linux:
                    windowInfo.SetAsChild(Handle, new CefRectangle(0, 0, 0, 0));
                    break;
                case CefRuntimePlatform.MacOSX:
                default:
                    break;
            }
            this.mCore.Create(windowInfo);

            this.mCore.AddressChanged += (s, e) =>
            {
                Gtk.Application.Invoke((_s, _e) => { });
            };
            this.mCore.BeforeClose += (s, e) =>
            {
                Gtk.Application.Invoke((_s, _e) => { });
            };
            this.mCore.BeforePopup += (s, e) =>
            {
                Gtk.Application.Invoke((_s, _e) => { });
            };
            this.mCore.BrowserCreated += (s, args) =>
            {
                this.mBrowserWindowHandle = this.mCore.Browser.GetHost().GetWindowHandle();
            };
            this.mCore.ConsoleMessage += (s, e) =>
            {
                Gtk.Application.Invoke((_s, _e) => { Log.Trace(e.Source, e.Line, e.Message); });
            };
            this.mCore.LoadEnd += (s, e) =>
            {
                Gtk.Application.Invoke((_s, _e) => { });
            };
            this.mCore.LoadError += (s, e) =>
            {
                Gtk.Application.Invoke((_s, _e) => { });
            };
            this.mCore.LoadingStateChange += (s, e) =>
            {
                Gtk.Application.Invoke((_s, _e) => { });
            };
            this.mCore.LoadStarted += (s, e) =>
            {
                Gtk.Application.Invoke((_s, _e) => { });
            };
            this.mCore.PluginCrashed += (s, e) =>
            {
                Gtk.Application.Invoke((_s, _e) => { });
            };
            this.mCore.RenderProcessTerminated += (s, e) =>
            {
                Gtk.Application.Invoke((_s, _e) => {
                    if (this.mCore != null)
                    {
                        Log.Critial("Render Process Terminated: reloading page");
                        if (this.mCore.Browser != null) this.mCore.Browser.Reload();
                    }
                });
            };
            this.mCore.StatusMessage += (s, e) =>
            {
                Gtk.Application.Invoke((_s, _e) => { });
            };
            this.mCore.PluginCrashed += (s, e) =>
            {
                Gtk.Application.Invoke((_s, _e) => { });
            };
            this.mCore.TitleChanged += (s, e) =>
            {
                Gtk.Application.Invoke((_s, _e) => {
                    if (!string.IsNullOrEmpty(e.Title)) Name = e.Title;
                });
            };
            this.mCore.Tooltip += (s, e) =>
            {
                Gtk.Application.Invoke((_s, _e) => { });
            };
            #endregion

            //if (File.Exists(Path.Combine(application.HostConfig.HostIconFile)))
            //    this.Icon = System.Drawing.Icon.FromHandle((new Bitmap(Path.Combine(application.HostConfig.HostIconFile))).GetHicon());
            //
            //this.BackColor = Color.FromArgb(32, 31, 41);


            this.Show();
        }

        #region OnInitialized
        public override void OnInitialized()
        {
        }
        #endregion


        #region Close

        /// <summary>
        /// The close.
        /// </summary>
        public void Close()
        {
            this.Dispose();
            this.mApplication.Quit();
        }

        #endregion Close/Dispose

        #region GetWindowSize
        public bool GetWindowSize()
        {
            return false;
        }
        #endregion

        #region CheckForUpdates
        public bool CheckForUpdates()
        {
            return false;
        }
        #endregion

        #region Update
        public bool Update()
        {
            return false;
        }
        #endregion
    }
}
