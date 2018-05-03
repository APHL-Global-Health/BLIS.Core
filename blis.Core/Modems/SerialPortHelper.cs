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

#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RJCP.IO.Ports;
using ePlug.Core.Extensions;
using ePlug.Core.Infrastructure;
using ePlug.Core.Configurations;
using ePlug.Core.Events;
using ePlug.Core.Modems;
#endregion

namespace ePlug.Core.Modems
{
    public class SerialPortHelper
    {
        #region Variables
        public AutoResetEvent receiveNow;
        public SerialPortStream port = null;
        private Configuration configuration;
        #endregion

        #region Constructor
        public SerialPortHelper()
        {
            configuration = Extension.GetConfiguration();
        }
        #endregion

        #region Open and Close Ports
        //Open Port
        public void Open(string p_strPortName, int p_uBaudRate = 115200, int p_uDataBits = 8, int p_uReadTimeout = 300, int p_uWriteTimeout = 300)
        {
            receiveNow = new AutoResetEvent(false);
			port = new SerialPortStream();

            try
            {
                port.PortName = p_strPortName;                 //COM1
                port.BaudRate = p_uBaudRate;                   //9600
                port.DataBits = p_uDataBits;                   //8
                port.StopBits = StopBits.One;                  //1
                port.Parity = Parity.None;                     //None
                port.ReadTimeout = p_uReadTimeout;             //300
                port.WriteTimeout = p_uWriteTimeout;           //300
                port.Encoding = Encoding.GetEncoding(1252);
				port.DataReceived += port_DataReceived; //new SerialDataReceivedEventHandler(port_DataReceived);
				port.Open();
                
            }
			catch (Exception ex) { 
				Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
        }

        //Close Port
        public void Close()
        {
            try
            {
                port.Close();
				port.DataReceived -= port_DataReceived;
                port = null;
            }
			catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
        }
        #endregion

        #region Execute AT Command
		public string ExecCommand(string command, int responseTimeout, string errorMessage)
        {
            try
            {
                port.DiscardOutBuffer();
                port.DiscardInBuffer();
                receiveNow.Reset();
				port.Write(command + "\r");

                string input = ReadResponse(responseTimeout);
				if ((input.Length == 0) || ((!input.EndsWith("\r\n> ")) && (!input.EndsWith("\r\nOK\r\n"))))
                    throw new ApplicationException("No success message was received.");
                return input;
            }
			catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
			return "";
        }
        #endregion

        #region Receive data from port
        public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (e.EventType == SerialData.Chars)
                {
                    receiveNow.Set();
                }
            }
			catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
        }

		public void port_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs e)
		{
			try
			{
				Console.WriteLine("Connected = {0}", e.Connected);
			}
			catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
		}

		public void port_MessageReceived(object sender, ReceivedMessageEventArgs e)
		{
			try
			{
				var message = System.Text.Encoding.Default.GetString(e.Data);
			}
			catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
		}
        #endregion

        #region ReadResponse
        public string ReadResponse(int timeout)
        {
            string buffer = string.Empty;
            try
            {
                do
                {
                    if (receiveNow.WaitOne(timeout, false))
                    {
                        string t = port.ReadExisting();
                        buffer += t;
						//Thread.Sleep(100);
                    }
                    else
                    {
                        if (buffer.Length > 0)
                            throw new ApplicationException("Response received is incomplete.");
                        else
                            throw new ApplicationException("No data received from phone.");
                    }
                }
                while (!buffer.EndsWith("\r\nOK\r\n") && !buffer.EndsWith("\r\n> ") && !buffer.EndsWith("\r\nERROR\r\n"));
            }
			catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
            return buffer;
        }
        #endregion

        #region TrimLineBreaks
        /// <summary>
		/// Removes all leading and trailing line termination characters from a string.
		/// </summary>
		/// <param name="input">The string to trim.</param>
		/// <returns>The modified string.</returns>
		private string TrimLineBreaks(string input)
        {
            char[] chrArray = new char[2];
            chrArray[0] = '\r';
            chrArray[1] = '\n';
            return input.Trim(chrArray);
        }
        #endregion

        #region IsSuccess
        private bool IsSuccess(string input)
        {
            return input.IndexOf("\r\nOK\r\n") >= 0;
        }
        #endregion

        #region IdentifyDevice
        public virtual Argument IdentifyDevice(List<Argument> settings, string PortName, iModemManager parent, out string SerialNumber)
        {
            Argument setting = null;
            SerialNumber = null;

            parent.WriteLine("Connecting to port " + PortName);
            if (port == null || (port != null && !port.IsOpen)) Open(PortName);

            if (port.IsOpen)
            {
                if (settings != null && !string.IsNullOrEmpty(PortName))
                {
                    try
                    {
                        parent.WriteLine("Looking for modem settings on port " + PortName);
                        string str1 = string.Concat("AT+CGSN", "\r");
                        var str2 = ExecCommand("AT", 300, "No phone connected at ");
                        if (this.IsSuccess(str2))
                        {
							str2 = ExecCommand("AT+CGSN", 300, "Failed to set message format.");
                            if (this.IsSuccess(str2))
                            {
                                if (str2.EndsWith("\r\nOK\r\n"))
                                {
                                    str2 = str2.Remove(str2.LastIndexOf("\r\nOK\r\n"), "\r\nOK\r\n".Length);
                                }
                                if (str2.StartsWith(str1))
                                {
                                    str2 = str2.Substring(str1.Length);
                                }
                            }

                            var serialNumber = TrimLineBreaks(str2);
                            if (!string.IsNullOrEmpty(serialNumber))
                            {
                                SerialNumber = serialNumber;
                                parent.WriteLine("Modem Serial Number is " + SerialNumber);

                                setting = settings.FirstOrDefault(ss => ss.Parameters.Any(pp => !string.IsNullOrEmpty(pp.Key) && pp.Value != null && pp.Key.ToLower().Equals("deviceserialnumber") && pp.Value.ToString().ToLower().Equals(serialNumber.ToLower())));
                                if (setting != null) parent.WriteLine("Settings found for " + setting.Name);
                                else parent.WriteLine("Settings not found for port " + PortName);
                            }
                        }
                    }
					catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
                }
            }
            else parent.WriteLine("Failed to port " + PortName);

			port.DataReceived -= port_DataReceived;
            return setting;
        }
        #endregion
    }
}
