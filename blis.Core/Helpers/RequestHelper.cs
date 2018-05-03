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

using blis.Helpers;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xilium.CefGlue;

namespace blis.Core.Helpers
{
    public class RequestHelper
    {
        private CefRequest request;
        private CefServer server;

        public RequestHelper()
        {
        }

        public RequestHelper(CefServer server, CefRequest request)
        {
            this.request = request;
            this.server = server;
            this.Url = request.Url;
            
            this.FirstPartyForCookies = request.FirstPartyForCookies;
            this.Identifier = (int)request.Identifier;
            this.IsReadOnly = request.IsReadOnly;
            this.Method = request.Method;
            this.Options = request.Options.ToString();

            var data = new PostHelper();

            if (request.PostData != null)
            {
                foreach (var element in request.PostData.GetElements())
                    data.Append(element);
            }

            this.PostData = data.Raw;
            this.ReferrerPolicy = request.ReferrerPolicy.ToString();
            this.ReferrerURL = request.ReferrerURL;
            this.ResourceType = request.ResourceType.ToString();
            this.TransitionType = request.TransitionType.ToString();
            this.Url = request.Url;
        }

        public string FirstPartyForCookies { get; set; }
        public string Url { get; set; }
        public int Identifier { get; set; }
        public bool IsReadOnly { get; set; }
        public string Method { get; set; }
        public string Options { get; set; }
        public string PostData { get; set; }
        public string ReferrerPolicy { get; set; }
        public string ReferrerURL { get; set; }
        public string ResourceType { get; set; }
        public string TransitionType { get; set; }

        public string ToJson()
        {
            return JsonMapper.ToJson(this);
        }

        public CefServer GetServer()
        {
            return server;
        }

        public CefRequest GetRequest()
        {
            return request;
        }
    }
}
