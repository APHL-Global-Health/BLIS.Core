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
using blis.Windows.Browser;
using blis.Core.Extensions;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Xilium.CefGlue;
using blis.Core.Browser;
using blis.Core.Infrastructure;

namespace blis.Windows.ChromeHost
{
    public class Window : Form
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
        {
            CefGlueClient.window = this;
            this.Text = application.HostConfig.HostTitle; 
            this.Size = new Size(application.HostConfig.HostWidth, application.HostConfig.HostHeight);
            this.StartPosition = FormStartPosition.CenterScreen;
            if (File.Exists(Path.Combine(application.HostConfig.HostIconFile)))
                this.Icon = Icon.FromHandle((new Bitmap(Path.Combine(application.HostConfig.HostIconFile))).GetHicon());

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScroll = false;
            this.BackColor = Color.FromArgb(32, 31, 41);

            this.mHostConfig = hostConfig;
            this.mApplication = application;

            var browserConfig = new CefBrowserConfig
            {
                StartUrl = application.HostConfig.StartUrl,
                ParentHandle = this.Handle,
                AppArgs = application.HostConfig.AppArgs,
                CefRectangle =
                    new CefRectangle
                    {
                        X = 0,
                        Y = 0,
                        Width = this.ClientSize.Width,
                        Height = this.ClientSize.Height
                    }
            };
            
            this.FormClosing += (s, args) => { this.mApplication.Quit(); };
            this.SizeChanged += (s, args) =>
            {
                if (this.mBrowserWindowHandle != IntPtr.Zero)
                {
                    NativeMethods.SetWindowPos(this.mBrowserWindowHandle, IntPtr.Zero, 0, 0, this.ClientSize.Width, this.ClientSize.Height, Core.Host.WinapiConstants.NoZOrder);
                }
            };

            #region Browser
            this.mCore = new CefGlueBrowser(browserConfig);            
            this.mCore.AddressChanged += (s, e) =>
            {
                this.InvokeIfRequired(() => { });
            };
            this.mCore.BeforeClose += (s, e) =>
            {
                this.InvokeIfRequired(() => { });
            };
            this.mCore.BeforePopup += (s, e) =>
            {
                this.InvokeIfRequired(() => { });
            };
            this.mCore.BrowserCreated += (s, args) =>
            {
                this.mBrowserWindowHandle = this.mCore.Browser.GetHost().GetWindowHandle();
            };
            this.mCore.ConsoleMessage += (s, e) =>
            {
                this.InvokeIfRequired(() => { Log.Trace(e.Source, e.Line, e.Message); });
            };
            this.mCore.LoadEnd += (s, e) =>
            {
                this.InvokeIfRequired(() => { });
            };
            this.mCore.LoadError += (s, e) =>
            {
                this.InvokeIfRequired(() => { });
            };
            this.mCore.LoadingStateChange += (s, e) =>
            {
                this.InvokeIfRequired(() => { });
            };
            this.mCore.LoadStarted += (s, e) =>
            {
                this.InvokeIfRequired(() => { });
            };
            this.mCore.PluginCrashed += (s, e) =>
            {
                this.InvokeIfRequired(() => { });
            };
            this.mCore.RenderProcessTerminated += (s, e) =>
            {
                this.InvokeIfRequired(() => {
                    if(this.mCore != null)
                    {
                        Log.Critial("Render Process Terminated: reloading page");
                        if(this.mCore.Browser != null) this.mCore.Browser.Reload();
                    }                    
                });
            };
            this.mCore.StatusMessage += (s, e) =>
            {
                this.InvokeIfRequired(() => { });
            };
            this.mCore.PluginCrashed += (s, e) =>
            {
                this.InvokeIfRequired(() => { });
            };
            this.mCore.TitleChanged += (s, e) =>
            {
                this.InvokeIfRequired(() => {
                    if (!string.IsNullOrEmpty(e.Title)) Text = e.Title;
                });
            };
            this.mCore.Tooltip += (s, e) =>
            {
                this.InvokeIfRequired(() => { });
            };
            #endregion
            
            this.Show();
        }
                
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
