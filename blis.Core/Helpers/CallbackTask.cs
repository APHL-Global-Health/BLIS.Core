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
using blis.Core.Extensions;
using System.Collections.Generic;
using Xilium.CefGlue;
#endregion

namespace blis.Core.Helpers
{
    public class CallbackTask : CefTask
    {
        #region Variables
        private CefV8Value cb;
        private CefV8Context cbcontext;
        private dynamic[] arguments;
        #endregion

        #region Constructor
        public CallbackTask(CefV8Context context, CefV8Value callback, params dynamic[] arguments)
        {
            this.cb = callback;
            this.cbcontext = context;
            this.arguments = arguments;
        }
        #endregion

        #region Execute
        protected override void Execute()
        {
            if (cb != null)
            {
                var args = new List<CefV8Value>();
                foreach (var arg in arguments) args.Add(Extension.ConvertToV8Value(arg));

                cb.ExecuteFunctionWithContext(cbcontext, null, args.ToArray());
            }
        }
        #endregion
    }
}
