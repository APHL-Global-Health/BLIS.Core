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
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace blis.Windows.ChromeHost
{
    using blis.Core;
    using System;
    using Xilium.CefGlue;

    /// <summary>
    /// The CefGlue browser host/window/app.
    /// </summary>
    public class CefGlueBrowserHost : HostBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueBrowserHost"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public CefGlueBrowserHost(CefConfiguration hostConfig)
            : base(hostConfig)
        {
        }

        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //private static extern bool SetProcessDPIAware();

        /// <summary>
        /// The platform initialize.
        /// </summary>
        protected override void PlatformInitialize()
        {
           // if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
        }

        /// <summary>
        /// The platform shutdown.
        /// </summary>
        protected override void PlatformShutdown()
        {
        }

        /// <summary>
        /// The platform run message loop.
        /// </summary>
        protected override void PlatformRunMessageLoop()
        {
            if (!MultiThreadedMessageLoop) System.Windows.Forms.Application.Idle += (s, e) => CefRuntime.DoMessageLoopWork();
            System.Windows.Forms.Application.Run();
        }

        /// <summary>
        /// The platform quit message loop.
        /// </summary>
        protected override void PlatformQuitMessageLoop()
        {
            System.Windows.Forms.Application.Exit();
        }

        /// <summary>
        /// The create main view.
        /// </summary>
        /// <returns>
        /// The <see cref="Window"/>.
        /// </returns>
        protected override Window CreateMainView()
        {
            return new Window(this, this.HostConfig);
        }
    }
}
