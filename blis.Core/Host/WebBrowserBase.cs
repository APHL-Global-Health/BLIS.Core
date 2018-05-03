﻿// --------------------------------------------------------------------------------------------------------------------
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

namespace blis.Core.Host
{
    using System;

    /// <summary>
    /// The web browser base.
    /// </summary>
    public abstract class WebBrowserBase : IDisposable
    {
        /// <summary>
        /// The disposed flag.
        /// </summary>
        private bool mDisposed;

        /// <summary>
        /// Finalizes an instance of the <see cref="WebBrowserBase"/> class. 
        /// </summary>
        ~WebBrowserBase()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// The dispose method.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The dispose method - checks if disposing flag is set.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.mDisposed)
            {
                return;
            }

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
            }

            this.mDisposed = true;
        }
    }
}