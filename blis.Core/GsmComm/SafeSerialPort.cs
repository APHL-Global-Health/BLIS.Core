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

using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using blis.Helpers;
using System.Reflection;
using Xilium.CefGlue;
using RJCP.IO.Ports;
using blis.Core.Events;
using blis.Core.Infrastructure;
using blis.Core.Configurations;

namespace blis.Core.GsmComm
{
	public class SafeSerialPort : SerialPortStream
    {        
		#region Variables
		private Stream theBaseStream;
		private bool messagingEnabled = true;
		// Read/Write error state variable
		private bool gotReadWriteError = true;

		// Serial port reader task
		private Thread reader;
		// Serial port connection watcher
		private Thread connectionWatcher;

		private object accessLock = new object();
		private bool disconnectRequested = false;

		private int responseTimeout = 300;
        private Configuration configuration;
        #endregion

        #region Events

        /// <summary>
        /// Connected state changed event.
        /// </summary>
        public delegate void ConnectionStatusChangedEventHandler(object sender, ConnectionStatusChangedEventArgs args);
		/// <summary>
		/// Occurs when connected state changed.
		/// </summary>
		public event ConnectionStatusChangedEventHandler ConnectionStatusChanged;

		/// <summary>
		/// Message received event.
		/// </summary>
		public delegate void MessageReceivedEventHandler(object sender, ReceivedMessageEventArgs args);
		/// <summary>
		/// Occurs when message received.
		/// </summary>
		public event MessageReceivedEventHandler ReceivedMessage;

		//public SerialDataReceivedEventHandler ReceivedData { get; set;}

		/// <summary>
		/// Raises the connected state changed event.
		/// </summary>
		/// <param name="args">Arguments.</param>
		protected virtual void OnConnectionStatusChanged(ConnectionStatusChangedEventArgs args)
		{
			//logger.Debug(args.Connected);
			if (ConnectionStatusChanged != null)
				ConnectionStatusChanged(this, args);
		}

		/// <summary>
		/// Raises the message received event.
		/// </summary>
		/// <param name="args">Arguments.</param>
		protected virtual void OnMessageReceived(ReceivedMessageEventArgs args)
		{
			//logger.Debug(BitConverter.ToString(args.Data));
			if (ReceivedMessage != null)
				ReceivedMessage(this, args);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether the serial port is connected.
		/// </summary>
		/// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
		public bool IsConnected
		{
			get { return !gotReadWriteError && !disconnectRequested; }
		}

		public bool MessagingEnabled 
		{
			get { return messagingEnabled; }
			set { messagingEnabled = value; }
		}

		public int ResponseTimeout 
		{
			get { return responseTimeout; }
			set { responseTimeout = value; }
		}
		#endregion

        public SafeSerialPort()
            : base()
        {
			MessagingEnabled = true;

            var path = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            if (File.Exists(Path.Combine(path, "Configurations.xml")))
                configuration = Configuration.Load(Path.Combine(path, "Configurations.xml"));
        }

        public SafeSerialPort(string portName)
            : base(portName)
        {
            var path = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            if (File.Exists(Path.Combine(path, "Configurations.xml")))
                configuration = Configuration.Load(Path.Combine(path, "Configurations.xml"));
        }

        /*public SafeSerialPort(IContainer container)
            : base(container)
        {

        }

        public SafeSerialPort(string portName, int baudRate)
            : base(portName, baudRate)
        {

        }

        public SafeSerialPort(string portName, int baudRate, Parity parity)
            : base(portName, baudRate, parity)
        {

        }

        public SafeSerialPort(string portName, int baudRate, Parity parity, int dataBits)
            : base(portName, baudRate, parity, dataBits)
        {

        }

        public SafeSerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
            : base(portName, baudRate, parity, dataBits, stopBits)
        {

        }*/

        /*public new void Open()
        {
            try
            {
				lock (accessLock)
				{
					Close();
					try
					{
						bool tryOpen = true;
                        if (CefRuntime.Platform != CefRuntimePlatform.Windows)
                        {
							tryOpen = (tryOpen && System.IO.File.Exists(this.PortName));
						}
						if (tryOpen){
							this.DtrEnable = true;
							this.RtsEnable = true;
							base.Open();
							//theBaseStream = BaseStream;
							//GC.SuppressFinalize(BaseStream);
						}
					}
					catch (Exception e)
					{
						//logger.Error(e);
						Close();
					}
					if (this.IsOpen)
					{
						gotReadWriteError = false;

                        if (CefRuntime.Platform != CefRuntimePlatform.Windows)
                        {
                            // Start the Reader task
                            reader = new Thread(ReaderTask);
                            reader.Start();
                        }
						OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(true));
					}
				}
            }
            catch
            {

            }
        }

		public new void Close()
		{
			lock (accessLock)
			{
				if (CefRuntime.Platform != CefRuntimePlatform.Windows && reader != null)
				{
					if (!reader.Join(5000))
						reader.Abort();
					reader = null;
				}
				if (this.IsOpen)
				{
					base.Close();
					OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(false));
				}
			}
		}*/

        public new void Dispose()
        {
            Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            /*if (disposing && (base.Container != null))
            {
                base.Container.Dispose();
            }
            try
            {
                if (theBaseStream.CanRead)
                {
                    theBaseStream.Close();
                    GC.ReRegisterForFinalize(theBaseStream);
                }
            }
			catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, ConfigurationHelper.Configurations.Verbose); }*/
            base.Dispose(disposing);
        }

		private void ReaderTask()
		{
			while (IsConnected)
			{
				int msglen = 0;
				//
				try
				{
					msglen = this.BytesToRead;
					if (msglen > 0)
					{	
						if(MessagingEnabled)
						{
							byte[] message = new byte[msglen];
							//
							int readbytes = 0;
							while (this.Read(message, readbytes, msglen - readbytes) <= 0)
								; // noop
							if (ReceivedMessage != null)
							{
								OnMessageReceived(new ReceivedMessageEventArgs(message));
							}
						}
						else
						{
							try
							{
								ConstructorInfo constructor = typeof (SerialDataReceivedEventArgs).GetConstructor(
									BindingFlags.NonPublic | BindingFlags.Instance,
									null,
									new[] {typeof (SerialData)},
									null);

								SerialDataReceivedEventArgs eventArgs = 
									(SerialDataReceivedEventArgs)constructor.Invoke(new object[] {SerialData.Chars});

								//if(ReceivedData != null)ReceivedData(this, eventArgs);
							}
							catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
							Thread.Sleep(responseTimeout);
						}
					}
					else
					{
						Thread.Sleep(100);
					}
				}
				catch (Exception e)
				{
					//logger.Error(e);
					gotReadWriteError = true;
					Thread.Sleep(1000);
				}
			}
		}

		private void ConnectionWatcherTask()
		{
			// This task takes care of automatically reconnecting the interface
			// when the connection is drop or if an I/O error occurs
			while (!disconnectRequested)
			{
				if (gotReadWriteError)
				{
					try
					{
						Close();
						// wait 1 sec before reconnecting
						Thread.Sleep(1000);
						if (!disconnectRequested)
						{
							try
							{
								Open();
							}
							catch (Exception e)
							{
								//logger.Error(e);
							}
						}
					}
					catch (Exception e)
					{
						//logger.Error(e);
					}
				}
				if (!disconnectRequested)
					Thread.Sleep(1000);
			}
		}
    }
}
