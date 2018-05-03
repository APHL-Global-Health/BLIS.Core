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

using GsmComm.Extensions;
using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace blis.Core.GsmComm.GsmCommunication
{
    internal class MessageIndicationHandlersEx
    {
        private const string deliverMemoryIndication = "\\+CMTI: \"(\\w+)\",(\\d+)";

        private const string deliverMemoryIndicationStart = "\\+CMTI: ";

        private const string deliverPduModeIndication = "\\+CMT: (\\w*),(\\d+)\\r\\n(\\w+)";

        private const string deliverPduModeIndicationStart = "\\+CMT: ";

        private const string statusReportMemoryIndication = "\\+CDSI: \"(\\w+)\",(\\d+)";

        private const string statusReportMemoryIndicationStart = "\\+CDSI: ";

        private const string statusReportPduModeIndication = "\\+CDS: (\\d+)\\r\\n(\\w+)";

        private const string statusReportPduModeIndicationStart = "\\+CDS: ";

        private const string ussdIndication = "\\+CUSD:\\s*(\\d+)(?:,\\s*\\\"([^\\\"]*)\\\"(?:,\\s*(\\d+))?)?";

        private const string ussdIndicationStart = "\\+CUSD: ";

        private List<MessageIndicationHandlersEx.UnsoMessage> messages;

        public MessageIndicationHandlersEx()
        {
            this.messages = new List<MessageIndicationHandlersEx.UnsoMessage>();
            MessageIndicationHandlersEx.UnsoMessage unsoMessage = new MessageIndicationHandlersEx.UnsoMessage("\\+CMTI: \"(\\w+)\",(\\d+)", new MessageIndicationHandlersEx.UnsoHandler(this.HandleDeliverMemoryIndication));
            unsoMessage.StartPattern = "\\+CMTI: ";
            unsoMessage.Description = "New SMS-DELIVER received (indicated by memory location)";
            this.messages.Add(unsoMessage);
            MessageIndicationHandlersEx.UnsoMessage unsoCompleteChecker = new MessageIndicationHandlersEx.UnsoMessage("\\+CMT: (\\w*),(\\d+)\\r\\n(\\w+)", new MessageIndicationHandlersEx.UnsoHandler(this.HandleDeliverPduModeIndication));
            unsoCompleteChecker.StartPattern = "\\+CMT: ";
            unsoCompleteChecker.Description = "New SMS-DELIVER received (indicated by PDU mode version)";
            unsoCompleteChecker.CompleteChecker = new MessageIndicationHandlersEx.UnsoCompleteChecker(this.IsCompleteDeliverPduModeIndication);
            this.messages.Add(unsoCompleteChecker);
            MessageIndicationHandlersEx.UnsoMessage unsoMessage1 = new MessageIndicationHandlersEx.UnsoMessage("\\+CDSI: \"(\\w+)\",(\\d+)", new MessageIndicationHandlersEx.UnsoHandler(this.HandleStatusReportMemoryIndication));
            unsoMessage1.StartPattern = "\\+CDSI: ";
            unsoMessage1.Description = "New SMS-STATUS-REPORT received (indicated by memory location)";
            this.messages.Add(unsoMessage1);
            MessageIndicationHandlersEx.UnsoMessage unsoCompleteChecker1 = new MessageIndicationHandlersEx.UnsoMessage("\\+CDS: (\\d+)\\r\\n(\\w+)", new MessageIndicationHandlersEx.UnsoHandler(this.HandleStatusReportPduModeIndication));
            unsoCompleteChecker1.StartPattern = "\\+CDS: ";
            unsoCompleteChecker1.Description = "New SMS-STATUS-REPORT received (indicated by PDU mode version)";
            unsoCompleteChecker1.CompleteChecker = new MessageIndicationHandlersEx.UnsoCompleteChecker(this.IsCompleteStatusReportPduModeIndication);
            this.messages.Add(unsoCompleteChecker1);

            MessageIndicationHandlersEx.UnsoMessage ussdMessage = new MessageIndicationHandlersEx.UnsoMessage("\\+CUSD:\\s*(\\d+)(?:,\\s*\\\"([^\\\"]*)\\\"(?:,\\s*(\\d+))?)?", new MessageIndicationHandlersEx.UnsoHandler(this.HandleUssdIndication));
            ussdMessage.StartPattern = "\\+CUSD: ";
            ussdMessage.Description = "New USSD received";
            ussdMessage.CompleteChecker = new MessageIndicationHandlersEx.UnsoCompleteChecker(this.IsCompleteUssdIndication);
            this.messages.Add(ussdMessage);

        }

        private IMessageIndicationObject HandleUssdIndication(ref string input)
        {
            Regex regex = new Regex("\\+CUSD:\\s*(\\d+)(?:,\\s*\\\"([^\\\"]*)\\\"(?:,\\s*(\\d+))?)?");
            Match match = regex.Match(input);
            if (match.Success)
            {
                int num = int.Parse(match.Groups[1].Value);
                string value = match.Groups[2].Value;
                int form = -1;
                if (!string.IsNullOrEmpty(match.Groups[3].Value) && !string.IsNullOrEmpty(match.Groups[3].Value.Trim()))
                    form = int.Parse(match.Groups[3].Value);

                ussdEx shortMessage = new ussdEx(num, value, form);
                input = input.Remove(match.Index, match.Length);
                return shortMessage;
            }
            else
            {
                throw new ArgumentException("Input string does not contain an SMS-DELIVER PDU mode indication.");
            }
        }

        private bool IsCompleteUssdIndication(string input)
        {
            Regex regex = new Regex("\\+CUSD:\\s*(\\d+)(?:,\\s*\\\"([^\\\"]*)\\\"(?:,\\s*(\\d+))?)?");
            Match match = regex.Match(input);
            if (match.Success)
            {
                return !string.IsNullOrEmpty(match.Groups[2].Value);
            }
            else
            {
                return false;
            }
        }

        private IMessageIndicationObject HandleDeliverMemoryIndication(ref string input)
        {
            Regex regex = new Regex("\\+CMTI: \"(\\w+)\",(\\d+)");
            Match match = regex.Match(input);
            if (match.Success)
            {
                string value = match.Groups[1].Value;
                int num = int.Parse(match.Groups[2].Value);
                MemoryLocation memoryLocation = new MemoryLocation(value, num);
                input = input.Remove(match.Index, match.Length);
                return memoryLocation;
            }
            else
            {
                throw new ArgumentException("Input string does not contain an SMS-DELIVER memory location indication.");
            }
        }

        private IMessageIndicationObject HandleDeliverPduModeIndication(ref string input)
        {
            Regex regex = new Regex("\\+CMT: (\\w*),(\\d+)\\r\\n(\\w+)");
            Match match = regex.Match(input);
            if (match.Success)
            {
                string value = match.Groups[1].Value;
                int num = int.Parse(match.Groups[2].Value);
                string str = match.Groups[3].Value;
                ShortMessage shortMessage = new ShortMessage(value, num, str);
                input = input.Remove(match.Index, match.Length);
                return shortMessage;
            }
            else
            {
                throw new ArgumentException("Input string does not contain an SMS-DELIVER PDU mode indication.");
            }
        }

        private IMessageIndicationObject HandleStatusReportMemoryIndication(ref string input)
        {
            Regex regex = new Regex("\\+CDSI: \"(\\w+)\",(\\d+)");
            Match match = regex.Match(input);
            if (match.Success)
            {
                string value = match.Groups[1].Value;
                int num = int.Parse(match.Groups[2].Value);
                MemoryLocation memoryLocation = new MemoryLocation(value, num);
                input = input.Remove(match.Index, match.Length);
                return memoryLocation;
            }
            else
            {
                throw new ArgumentException("Input string does not contain an SMS-STATUS-REPORT memory location indication.");
            }
        }

        private IMessageIndicationObject HandleStatusReportPduModeIndication(ref string input)
        {
            Regex regex = new Regex("\\+CDS: (\\d+)\\r\\n(\\w+)");
            Match match = regex.Match(input);
            if (match.Success)
            {
                int num = int.Parse(match.Groups[1].Value);
                string value = match.Groups[2].Value;
                ShortMessage shortMessage = new ShortMessage(string.Empty, num, value);
                input = input.Remove(match.Index, match.Length);
                return shortMessage;
            }
            else
            {
                throw new ArgumentException("Input string does not contain an SMS-STATUS-REPORT PDU mode indication.");
            }
        }

        /// <summary>
        /// Handles an unsolicited message of the specified input string.
        /// </summary>
        /// <param name="input">The input string to handle, the unsolicited message will be removed</param>
        /// <param name="description">Receives a textual description of the message, may be empty</param>
        /// <returns>The message indication object generated from the message</returns>
        /// <exception cref="T:System.ArgumentException">Input string does not match any of the supported
        /// unsolicited messages</exception>
        public IMessageIndicationObject HandleUnsolicitedMessage(ref string input, out string description)
        {
            IMessageIndicationObject messageIndicationObject;
            List<MessageIndicationHandlersEx.UnsoMessage>.Enumerator enumerator = this.messages.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    MessageIndicationHandlersEx.UnsoMessage current = enumerator.Current;
                    if (!current.IsMatch(input))
                    {
                        continue;
                    }
                    IMessageIndicationObject messageIndicationObject1 = current.Handler(ref input);
                    description = current.Description;
                    messageIndicationObject = messageIndicationObject1;
                    return messageIndicationObject;
                }
                throw new ArgumentException("Input string does not match any of the supported unsolicited messages.");
            }
            finally
            {
                enumerator.Dispose();
            }
            return messageIndicationObject;
        }

        private bool IsCompleteDeliverPduModeIndication(string input)
        {
            Regex regex = new Regex("\\+CMT: (\\w*),(\\d+)\\r\\n(\\w+)");
            Match match = regex.Match(input);
            if (match.Success)
            {
                //match.Groups[1].Value;
                int num = int.Parse(match.Groups[2].Value);
                string value = match.Groups[3].Value;
                if (BcdWorker.CountBytes(value) <= 0)
                {
                    return false;
                }
                else
                {
                    int num1 = BcdWorker.GetByte(value, 0);
                    int num2 = num * 2 + num1 * 2 + 2;
                    return value.Length >= num2;
                }
            }
            else
            {
                return false;
            }
        }

        private bool IsCompleteStatusReportPduModeIndication(string input)
        {
            Regex regex = new Regex("\\+CDS: (\\d+)\\r\\n(\\w+)");
            Match match = regex.Match(input);
            if (match.Success)
            {
                int num = int.Parse(match.Groups[1].Value);
                string value = match.Groups[2].Value;
                if (BcdWorker.CountBytes(value) <= 0)
                {
                    return false;
                }
                else
                {
                    int num1 = BcdWorker.GetByte(value, 0);
                    int num2 = num * 2 + num1 * 2 + 2;
                    return value.Length >= num2;
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsIncompleteUnsolicitedMessage(string input)
        {
            bool flag = false;
            foreach (MessageIndicationHandlersEx.UnsoMessage message in this.messages)
            {
                if (!message.IsStartMatch(input) || message.IsMatch(input))
                {
                    continue;
                }
                flag = true;
                break;
            }
            return flag;
        }

        public bool IsUnsolicitedMessage(string input)
        {
            bool flag = false;
            foreach (MessageIndicationHandlersEx.UnsoMessage message in this.messages)
            {
                if (!message.IsMatch(input))
                {
                    continue;
                }
                flag = true;
                break;
            }
            return flag;
        }

        private delegate bool UnsoCompleteChecker(string input);

        private delegate IMessageIndicationObject UnsoHandler(ref string input);

        private class UnsoMessage
        {
            private string pattern;

            private string startPattern;

            private string description;

            private MessageIndicationHandlersEx.UnsoHandler handler;

            private MessageIndicationHandlersEx.UnsoCompleteChecker completeChecker;

            public MessageIndicationHandlersEx.UnsoCompleteChecker CompleteChecker
            {
                get
                {
                    return this.completeChecker;
                }
                set
                {
                    this.completeChecker = value;
                }
            }

            public string Description
            {
                get
                {
                    return this.description;
                }
                set
                {
                    this.description = value;
                }
            }

            public MessageIndicationHandlersEx.UnsoHandler Handler
            {
                get
                {
                    return this.handler;
                }
                set
                {
                    this.handler = value;
                }
            }

            public string Pattern
            {
                get
                {
                    return this.pattern;
                }
                set
                {
                    this.pattern = value;
                }
            }

            public string StartPattern
            {
                get
                {
                    return this.startPattern;
                }
                set
                {
                    this.startPattern = value;
                }
            }

            public UnsoMessage(string pattern, MessageIndicationHandlersEx.UnsoHandler handler)
            {
                this.pattern = pattern;
                this.startPattern = pattern;
                this.description = string.Empty;
                this.handler = handler;
                this.completeChecker = null;
            }

            public bool IsMatch(string input)
            {
                bool flag;
                if (this.completeChecker == null)
                {
                    flag = Regex.IsMatch(input, this.pattern);
                }
                else
                {
                    flag = this.completeChecker(input);
                }
                return flag;
            }

            public bool IsStartMatch(string input)
            {
                return Regex.IsMatch(input, this.startPattern);
            }
        }
    }
}
