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

namespace ePlug.Core.Modems.Messages
{
    public class ShortMessage : Message
    {
        #region Properties
        public string Index { get; private set; }
        public string Message { get; private set; }
        public string Storage { get; set; }
        public string Sender { get; private set; }
        public string ServiceCenterNumber { get; private set; }
        public string SentOn { get; set; }

        public int CurrentIndex { get; private set; }
        public int TotalMessages { get; private set; }

        public override string MessageType { get { return "ShortMessage"; } }
        #endregion

        #region Constructor
        public ShortMessage(string Index, string Message, string Storage, string Sender, string ServiceCenterNumber, string SentOn, int CurrentIndex = 1, int TotalMessages = 1)
        {
            this.Index = Index;
            this.Message = Message;
            this.Storage = Storage;
            this.Sender = Sender;
            this.ServiceCenterNumber = ServiceCenterNumber;
            this.SentOn = SentOn;

            this.CurrentIndex = CurrentIndex;
            this.TotalMessages = TotalMessages;
        }
        #endregion
    }
}
