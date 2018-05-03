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

using Xilium.CefGlue;

namespace blis.Core.Browser.Handlers
{
    public class CefGlueFocusHandler : CefFocusHandler
    {
        protected override void OnGotFocus(CefBrowser browser)
        {
            //Console.WriteLine ("OnGotFocus");
            //System.Diagnostics.Debug.WriteLine(", OnGotFocus"); 
            base.OnGotFocus(browser);
        }

        protected override bool OnSetFocus(CefBrowser browser, CefFocusSource source)
        {
            //Console.WriteLine ("OnSetFocus");
            //System.Diagnostics.Debug.WriteLine(", OnSetFocus: " + source.ToString()); 
            return base.OnSetFocus(browser, source);
        }

        protected override void OnTakeFocus(CefBrowser browser, bool next)
        {
            //Console.WriteLine ("OnTakeFocus");
            //System.Diagnostics.Debug.WriteLine(", OnTakeFocus"); 
            base.OnTakeFocus(browser, next);
        }
    }
}
