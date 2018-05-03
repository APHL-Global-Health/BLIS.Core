// --------------------------------------------------------------------------------------------------------------------
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
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// This is a port from CefGlue.WindowsForms sample of CefGlue. Mostly provided as-is. 
// For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
// </note>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace ePlug.Core.Modems.Messages
{
    public class LongMessage : Message
    {
        public override string MessageType { get { return "LongMessage"; } }

        public bool IsComplete { get; private set; }
        public string Message { get { return CombineMessages(); } }
        public List<ShortMessage> Messages { get; private set; }

        public LongMessage(int Reference, bool IsComplete)
        {
            this.Reference = Reference;
            this.IsComplete = IsComplete;
            this.Messages = new List<ShortMessage>();
        }

        public void Add(ShortMessage item)
        {
            Messages.Add(item);
        }

        public string CombineMessages()
        {
            return string.Join("", Messages.OrderBy(ss => ss.TotalMessages).ThenBy(ss => ss.CurrentIndex).Select(s => s.Message.Length > 6 ? s.Message.Substring(7) : s.Message));//.Where(s => s.Message.Length > 6)
        }
    }
}
