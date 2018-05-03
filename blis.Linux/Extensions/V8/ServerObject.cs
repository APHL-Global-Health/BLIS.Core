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

using blis.Core.Configurations;
using blis.Core.Handlers;
using System;
using System.IO;
using System.Reflection;
using V8.Net;
using Xilium.CefGlue;

namespace blis.Linux.Extensions.V8
{
    public class ServerObject : V8NativeObject
    {
        #region Variables
        private Configuration configuration;
        #endregion

        #region Initialize
        public override InternalHandle Initialize(bool isConstructCall, params InternalHandle[] args)
        {
            var path = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            if (File.Exists(Path.Combine(path, "Configurations.xml")))
                configuration = Configuration.Load(Path.Combine(path, "Configurations.xml"));

            this.AsDynamic.create = Engine.CreateFunctionTemplate().GetFunctionObject(create);

            return base.Initialize(isConstructCall, args);
        }
        #endregion

        #region create 
        private InternalHandle create(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (args.Length == 3)
            {
                var address = args[0].AsString;
                var port    = args[1].AsInt32;
                var threads = args[2].AsInt32;
                if (!string.IsNullOrEmpty(address) && port > 0 && threads > 0)
                {
                    var serverHandler = new ServerHandler();
                    CefServer.Create(address, (ushort)port, threads, serverHandler);
                }
            }

            return null;
        }
        #endregion
    }
}
