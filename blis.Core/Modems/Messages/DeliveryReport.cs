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

namespace ePlug.Core.Modems.Messages
{
    public class DeliveryReport : Message
    {
        public string Data { get; private set; }

        public string TimeStamp { get; private set; }
        public string Discharge { get; private set; }
        public string Status { get; private set; }
        public string SMSC { get; private set; }
        public string Number { get; private set; }
        public int Index { get; private set; }
        public string Storage { get; private set; }

        public override string MessageType { get { return "DeliveryReport"; } }

        public DeliveryReport(int Index, string Storage, string TimeStamp, string Discharge, string Status, string SMSC, int Reference, string Number)
        {
            this.TimeStamp = TimeStamp;
            this.Discharge = Discharge;
            this.Status = Status;
            this.SMSC = SMSC;
            this.Reference = Reference;
            this.Number = Number;

            this.Index = Index;
            this.Storage = Storage;
        }

        public DeliveryReport(string Data)
        {
            this.Data = Data;

            var _SMSC = this.Data.Substring(0, 16);
            var TPMR = this.Data.Substring(18, 2);
            var TPRA = this.Data.Substring(20, 14);
            var TPSCTS = this.Data.Substring(34, 14);
            var TPDT = this.Data.Substring(48, 14);
            var TPST = this.Data.Substring(62, 2);

            Number = Reverse(TPRA.Substring(4), 10);
            Reference = Convert.ToInt32(TPMR, 16);

            SMSC = Reverse(_SMSC.Substring(4), 12);
            Status = "SMS-STATUS-REPORT";
            TimeStamp = ToDateTime(Reverse(TPSCTS, 12));
            Discharge = ToDateTime(Reverse(TPDT, 12));
        }

        private string Reverse(string data, int len)
        {
            var r = "";
            for (int x = 0; x < len; x += 2)
            {
                var c = data[x];
                var n = data[x + 1];
                r += n.ToString() + c.ToString();
            }
            return r;
        }

        private string ToDateTime(string data)
        {
            var year = data.Substring(0, 2);
            var month = data.Substring(2, 2);
            var day = data.Substring(4, 2);
            var hr = data.Substring(6, 2);
            var min = data.Substring(8, 2);
            var sec = data.Substring(10, 2);

            return day + "/" + month + "/" + year + " " + hr + ":" + min + ":" + sec;
        }
    }
}
