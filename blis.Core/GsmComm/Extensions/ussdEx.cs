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

using GsmComm.GsmCommunication;
using System;
using System.Xml.Serialization;

namespace GsmComm.Extensions
{
    [Serializable]
    //[XmlInclude(typeof(ShortMessageFromPhone))]
    public class ussdEx : IMessageIndicationObject
    {
        private int indicator;

        private string data;

        private int format;

        /// <summary>
		/// The indicator.
		/// </summary>
		[XmlAttribute]
        public int Indicator
        {
            get
            {
                return this.indicator;
            }
            set
            {
                this.indicator = value;
            }
        }

        /// <summary>
		/// The actual message.
		/// </summary>
		[XmlElement]
        public string Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }

        /// <summary>
		/// The format in which the message is encoded.
		/// </summary>
        [XmlElement]
        public int Format
        {
            get
            {
                return this.format;
            }
            set
            {
                this.format = value;
            }
        }

        /// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public ussdEx()
        {
            this.indicator = -1;
            this.data = string.Empty;
            this.format = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GsmComm.Extensions.USSDMessage" /> class.
        /// </summary>
        /// <param name="indicator">The indicator</param>
        /// <param name="data">The message</param>
        /// <param name="format">The format in which the message is encoded</param>
        public ussdEx(int indicator, string data, int format)
        {
            this.indicator = indicator;
            this.data = data;
            this.format = format;


        }
    }
}
