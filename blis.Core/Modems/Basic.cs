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
using ePlug.Core.Configurations;
using ePlug.Core.Events;
using ePlug.Core.Extensions;
using ePlug.Core.Infrastructure;
using ePlug.Core.Modems.Messages;
using GsmComm.Extensions;
using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using GsmComm.PduConverter.SmartMessaging;
using RJCP.IO.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
#endregion

namespace ePlug.Core.Modems
{
    public class Basic
    {
        #region Variables
        public GsmCommMainEx comm;
        public EventHandler<iEventArgs> iEvents;
        public List<Argument> settings;
        public iModemManager parent;
        public bool isBusy = false;
        public int Timeout = 300;

        public Configuration configuration;
        #endregion

        #region Properties
        //public DeviceInfo Info { get; private set; }
        public string PortName { get; private set; }
		public SerialPortStream Port { get; private set; }
        public Argument Setting { get; set; }
        public virtual string Name
        {
            get
            {
                if (Setting != null) return Setting.Name;
                else return "Basic";
            }
        }
        #endregion

        #region Constructor
        public Basic(Argument setting, List<Argument> settings, string PortName, iModemManager parent)
            :this(null, setting, settings, PortName, parent)
        {
        }

		public Basic(SerialPortStream port, Argument setting, List<Argument> settings, string PortName, iModemManager parent)
        {
            if (PortName != null) this.PortName = PortName;
            if (setting != null) this.Setting = setting;
            if (settings != null) this.settings = settings;
            this.Port = port;
            this.parent = parent;

            configuration = Extension.GetConfiguration();
        }
        #endregion

        #region IdentifyDevice
        public virtual Argument IdentifyDevice()
        {
            Argument setting = null;
            
            if (this.settings != null && !string.IsNullOrEmpty(PortName))
            {
                int baud = 115200;

                GsmCommMainEx idComm = new GsmCommMainEx("COM" + this.PortName, baud, Timeout);
                if (this.Port != null) idComm = new GsmCommMainEx("COM" + this.PortName, baud, Timeout);

                this.parent.WriteLine("Connecting to port " + this.PortName);
                try { if(!idComm.IsOpen()) idComm.Open(); }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
            
                if (idComm.IsOpen())
                {
                    this.parent.WriteLine("Looking for modem settings on port " + this.PortName);
                    var SerialNumber = idComm.RequestSerialNumber();
                    if (!string.IsNullOrEmpty(SerialNumber))
                    {
                        this.parent.WriteLine("Modem Serial Number is " + SerialNumber);
            
                        setting = this.settings.FirstOrDefault(ss => ss.Parameters.Any(pp => !string.IsNullOrEmpty(pp.Key) && pp.Value != null && pp.Key.ToLower().Equals("deviceserialnumber") && pp.Value.ToString().ToLower().Equals(SerialNumber.ToLower())));
                        if(setting != null)this.parent.WriteLine("Settings found for " + setting.Name);
                        else this.parent.WriteLine("Settings not found for port " + this.PortName);
                    }
                    idComm.ForceClose();
                }
                else this.parent.WriteLine("Failed to port " + this.PortName);
            }

            return setting;
        }
        #endregion

        #region Dispose
        public virtual void Dispose(bool forcefully = false)
        {
            try
            {
                if(forcefully) HangUp();
                if (comm != null)comm.ForceClose();
                 comm = null;
                if (!forcefully && parent != null) parent.RemoveGSMDevice(this);                
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
        }
        #endregion

        #region Initialize
        public virtual void Initialize()
        {
            if (!string.IsNullOrEmpty(PortName))
            {
                int baud = 115200;
                //Log.Trace(Info.ToString(), configuration.Verbose);
                if (parent != null && Setting != null)
                {
                    parent.WriteLine("[" + Setting.Name + "]----------------------------------------");
                    parent.WriteLine(string.Format("[{0}] {1}: {2}", Setting.Name, "Modem Name", Name));
                    //parent.WriteLine(string.Format("[{0}] {1}: {2}", Setting.Name, "Manufacturer", Info.Manufacturer));
                    //parent.WriteLine(string.Format("[{0}] {1}: {2}", Setting.Name, "Name", Info.Name));
                    parent.WriteLine(string.Format("[{0}] {1}: {2}", Setting.Name, "Port", PortName));
                    //parent.WriteLine(string.Format("[{0}] {1}: {2}", Setting.Name, "Status", Info.Status));
                    parent.WriteLine("["+ Setting.Name + "]----------------------------------------");

                    var baudrate = Setting.Parameters.FirstOrDefault(ss => !string.IsNullOrEmpty(ss.Key) && ss.Key.ToLower().Equals("baudrate"));
                    if (baudrate != null && baudrate.Value != null && int.TryParse(baudrate.Value.ToString(), out baud))
                    {
                        //has baud rate settings
                    }
                }

                var forcefully = false;
                comm = new GsmCommMainEx(this.PortName, baud, Timeout);
                if (this.Port != null)
                {
                    comm = new GsmCommMainEx(this.Port, Timeout);
                    forcefully = true;
                }


                try { comm.Open(forcefully); }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }

                if (comm.IsOpen())
                {
                    comm.MessageReceived += MessageReceived;
                    comm.MessageSendComplete += MessageSendComplete;
                    comm.MessageSendFailed += MessageSendFailed;
                    comm.MessageSendStarting += MessageSendStarting;
                    comm.PhoneConnected += PhoneConnected;
                    comm.PhoneDisconnected += PhoneDisconnected;
                    comm.ReceiveComplete += ReceiveComplete;
                    comm.ReceiveProgress += ReceiveProgress;
                    comm.LoglineAdded       += LoglineAdded;

                    try
                    {
                        comm.EnableMessageNotifications();
                        //ReadAllMessages(); /*ReadMessagesFromStorages(new List<string>() { "SIM", "PHONE" });*/
                    }
                    catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
                }
                else comm = null;
            }
        }
        #endregion

        #region Send SMS
        public virtual bool SendMessage(string destination, string message, ref int MessageReference, string source = "", int current = 0, int retries = 10)
        {
            bool isSent = false;
            
            try
            {
                if (comm != null && comm.IsOpen())
                {
                    string data = TextDataConverter.StringTo7Bit(message);
                    int length = data.Length;
                    if (length > 160)
                    {
                        SmsSubmitPdu[] smses = SmartMessageFactory.CreateConcatTextMessage(message, false, destination);
                        foreach (var _sms in smses) _sms.RequestStatusReport = true;
                        comm.SendMessages(smses);
                        var sms = smses.FirstOrDefault();
                        if(sms != null)
                        {
                            MessageReference = sms.MessageReference;
                            var sent = new SentMessage(message, sms.MessageReference, sms.DestinationAddress);
                            sent.SerialNumber = GetSerial();
                            if (iEvents != null) iEvents(this, new iEventArgs(sent));
                        }
                    }
                    else
                    {                        
                        byte dcs = (byte)DataCodingScheme.GeneralCoding.Alpha7BitDefault;
                        //byte dcs = (byte)DataCodingScheme.GeneralCoding.AlphaReserved;
                        //byte dcs = (byte)DataCodingScheme.GeneralCoding.Class0; //Shows as popup, can cancel or save
                        //byte dcs = (byte)DataCodingScheme.GeneralCoding.Class1;
                        //byte dcs = (byte)DataCodingScheme.GeneralCoding.Class2;
                        //byte dcs = (byte)DataCodingScheme.GeneralCoding.Class3;
                        //byte dcs = (byte)DataCodingScheme.GeneralCoding.NoClass;
                        //byte dcs = (byte)DataCodingScheme.GeneralCoding.Uncompressed;

                        var sms = new SmsSubmitPdu(message, destination, source, dcs) { RequestStatusReport = true };
                        comm.SendMessage(sms);

                        MessageReference = sms.MessageReference;
                        var sent = new SentMessage(message, sms.MessageReference, sms.DestinationAddress);
                        sent.SerialNumber = GetSerial();
                        if (iEvents != null) iEvents(this, new iEventArgs(sent));
                    }
                    isSent = true;
                }
            }
            catch (Exception ex)
            {
                Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose);
                if (ex.Message == "Message service error 500 occurred.")
                {
                    if (!isSent && current < retries)
                    {
                        Thread.Sleep(10);
                        isSent = SendMessage(destination, message, ref MessageReference, source, ++current, retries);
                    }
                }
                
            }

            return isSent;
        }
        #endregion

        #region MessageReceived
        public virtual void MessageReceived(object s, MessageReceivedEventArgs e)
        {
            try
            {
                IMessageIndicationObject obj = e.IndicationObject;
                if (obj != null)
                {
                    if (obj is ussdEx)
                    {
                        var item = (ussdEx)obj;

                        string message = string.Empty;
                        var bytes = Calc.HexToInt(item.Data);

                        try { message = PduParts.DecodeUcs2Text(bytes); }
                        catch { try { message = PduParts.Decode7BitText(bytes); } catch { } }

                        var _msg = new USSDMessage(item.Indicator, message, item.Format);
                        _msg.SerialNumber = GetSerial();

                        if (iEvents != null) iEvents(s, new iEventArgs(_msg));
                    }
                    else
                    {
                        MemoryLocation loc = (MemoryLocation)obj;
                        DecodedShortMessage message = comm.ReadMessage(loc.Index, loc.Storage);
                        if (message != null)
                        {
                            SmsPdu smsrec = message.Data;
                            if (smsrec is SmsStatusReportPduEx)
                            {
                                SmsStatusReportPduEx msg = (SmsStatusReportPduEx)smsrec;
                                var _msg = new DeliveryReport(message.Index, message.Storage, msg.SCTimestamp.ToDateTime().ToString(), msg.DischargeTime.ToDateTime().ToString(), msg.Status.Category.ToString(), msg.SmscAddress, msg.MessageReference, msg.RecipientAddress);
                                _msg.SerialNumber = GetSerial();

                                if (iEvents != null) iEvents(s, new iEventArgs(_msg));
                            }
                            else if (smsrec is SmsStatusReportPdu)
                            {
                                SmsStatusReportPdu msg = (SmsStatusReportPdu)smsrec;
                                var _msg = new DeliveryReport(message.Index, message.Storage, msg.SCTimestamp.ToDateTime().ToString(), msg.DischargeTime.ToDateTime().ToString(), msg.Status.Category.ToString(), msg.SmscAddress, msg.MessageReference, msg.RecipientAddress);
                                _msg.SerialNumber = GetSerial();

                                if (iEvents != null) iEvents(s, new iEventArgs(_msg));
                            }
                            else if (smsrec is SmsDeliverPdu)
                            {
                                SmsDeliverPdu msg = (SmsDeliverPdu)smsrec;

                                if (SmartMessageDecoder.IsPartOfConcatMessage(smsrec))
                                {
                                    var info = SmartMessageDecoder.GetConcatenationInfo(smsrec);
                                    var sms = new LongMessage(info.ReferenceNumber, false);
                                    sms.Add(new ePlug.Core.Modems.Messages.ShortMessage(message.Index.ToString(), msg.UserDataText, message.Storage, msg.OriginatingAddress, msg.SmscAddress, smsrec.GetTimestamp().ToDateTime().ToString(), info.CurrentNumber, info.TotalMessages));
                                    sms.CombineMessages();
                                    sms.SerialNumber = GetSerial();

                                    if (iEvents != null) iEvents(s, new iEventArgs(sms));
                                }
                                else
                                {
                                    var _msg = new ePlug.Core.Modems.Messages.ShortMessage(message.Index.ToString(), msg.UserDataText, message.Storage, msg.OriginatingAddress, msg.SmscAddress, smsrec.GetTimestamp().ToDateTime().ToString());
                                    _msg.SerialNumber = GetSerial();

                                    if (iEvents != null) iEvents(s, new iEventArgs(_msg));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
            
        }
        #endregion

        #region Receive Progress
        public virtual void ReceiveProgress(object sender, ProgressEventArgs e)
        {

        }
        #endregion

        #region Receive Complete
        public virtual void ReceiveComplete(object sender, ProgressEventArgs e)
        {

        }
        #endregion

        #region Phone Disconnected
        public virtual void PhoneDisconnected(object sender, EventArgs e)
        {
            Dispose();
        }
        #endregion

        #region Phone Connected
        public virtual void PhoneConnected(object sender, EventArgs e)
        {

        }
        #endregion

        #region Message Send Starting
        public virtual void MessageSendStarting(object sender, MessageEventArgs e)
        {

        }
        #endregion

        #region Message Send Failed
        public virtual void MessageSendFailed(object sender, MessageErrorEventArgs e)
        {

        }
        #endregion

        #region Message Send Complete
        public virtual void MessageSendComplete(object sender, MessageEventArgs e)
        {
            
        }
        #endregion

        #region LoglineAdded
        public virtual void LoglineAdded(object sender, LoglineAddedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Text))
            {
                Log.Trace(e.Text, configuration.Verbose);
                var text = e.Text;

                if(text.Contains("[gsmphone]"))
                    text = text.Substring(text.IndexOf("[gsmphone]") + 10).Replace(Environment.NewLine, "").Trim();
                if (parent != null && Setting != null && !string.IsNullOrEmpty(text)) parent.WriteLine("[" + Setting.Name + "] " + text);
            }
        }
        #endregion
        
        #region QueryModem
        public virtual Dictionary<string, object> QueryModem(ModemQueries query, object command = null)
        {
            var response = new Dictionary<string, object>();
            if (comm != null && comm.IsOpen())
            {
                #region Request USSD
                if (query == ModemQueries.RequestUSSD)
                {
                    var isString = command is string;
                    if (comm != null && comm.IsOpen() && command != null && command is string)
                    {
                        string info = "";
                        var request = command.ToString();
                        var result = ProcessUSSD(request, ref info);
                        response.Add("USSD", result);
                    }
                }
                #endregion

                #region Cancel USSD
                else if (query == ModemQueries.CancelUSSD)
                {
                    comm.SendUssdRequest(2, "");
                }
                #endregion

                #region Get Signal Quality
                else if (query == ModemQueries.GetSignalQuality)
                {
                    var signal = comm.GetSignalQuality();
                    if (signal != null)
                    {
                        response.Add("BER", signal.BitErrorRate);
                        response.Add("RSSI", signal.SignalStrength);
                    }
                }
                #endregion

                #region Get Current Operator
                else if (query == ModemQueries.GetCurrentOperator)
                {
                    var opperator = comm.GetCurrentOperator();
                    if (opperator != null)
                    {
                        response.Add("AccessTechnology", opperator.AccessTechnology);
                        response.Add("Operator", opperator.TheOperator);
                        response.Add("Format", opperator.Format.ToString());
                    }
                }
                #endregion
                
                #region Delete Messages
                else if (query == ModemQueries.DeleteMessages)
                {
                    if (command != null && command is Dictionary<string, object>)
                    {
                        var dict = (Dictionary<string, object>)command;
                        var status = DeleteScope.All;
                        var storage = PhoneStorageType.Sim;
                        
                        if (dict.Keys.Contains("Status"))
                        {
                            if (dict["Status"] != null)
                            {
                                if (dict["Status"] is int)
                                {
                                    if ((int)dict["Status"] == 0) status = DeleteScope.All;
                                    else if ((int)dict["Status"] == 1) status = DeleteScope.Read;
                                    else if ((int)dict["Status"] == 2) status = DeleteScope.ReadAndSent;
                                    else if ((int)dict["Status"] == 3) status = DeleteScope.ReadSentAndUnsent;
                                }
                                else if (dict["Status"] is string)
                                {
                                    if (dict["Status"].ToString().ToUpper() == "ALL") status = DeleteScope.All;
                                    else if (dict["Status"].ToString().ToUpper() == "READ") status = DeleteScope.Read;
                                    else if (dict["Status"].ToString().ToUpper() == "READANDSENT") status = DeleteScope.ReadAndSent;
                                    else if (dict["Status"].ToString().ToUpper() == "READSENTANDUNSENT") status = DeleteScope.ReadSentAndUnsent;
                                }
                            }
                        }
                        
                        if (dict.Keys.Contains("Storage"))
                        {
                            if (dict["Storage"] != null)
                            {
                                if (dict["Storage"] is int)
                                {
                                    if ((int)dict["Storage"] == 0) storage = PhoneStorageType.Sim;
                                    else if ((int)dict["Storage"] == 1) storage = PhoneStorageType.Phone;
                                }
                                else if (dict["Storage"] is string)
                                {
                                    if (dict["Storage"].ToString().ToUpper() == "SIM") storage = PhoneStorageType.Sim;
                                    else if (dict["Storage"].ToString().ToUpper() == "PHONE") storage = PhoneStorageType.Phone;
                                    else if (!string.IsNullOrEmpty(dict["Storage"].ToString().Trim())) storage = dict["Storage"].ToString().ToUpper();
                                }
                            }
                        }

                        comm.DeleteMessages(status, storage);
                        response.Add("Status", "Ok");
                    }
                    else
                    {
                        ReadAllMessages();
                        response.Add("Status", "Ok");
                    }
                }
                #endregion

                #region Read Messages
                else if (query == ModemQueries.ReadMessages)
                {
                    if (command != null && command is Dictionary<string, object>)
                    {
                        var dict = (Dictionary<string, object>)command;
                        var status = PhoneMessageStatus.All;
                        var storage = PhoneStorageType.Sim;

                        if (dict.Keys.Contains("Status"))
                        {
                            if (dict["Status"] != null)
                            {
                                if (dict["Status"] is int)
                                {
                                    if ((int)dict["Status"] == 0) status = PhoneMessageStatus.All;
                                    else if ((int)dict["Status"] == 1) status = PhoneMessageStatus.ReceivedRead;
                                    else if ((int)dict["Status"] == 2) status = PhoneMessageStatus.ReceivedUnread;
                                    else if ((int)dict["Status"] == 3) status = PhoneMessageStatus.StoredSent;
                                    else if ((int)dict["Status"] == 4) status = PhoneMessageStatus.StoredUnsent;
                                }
                                else if (dict["Status"] is string)
                                {
                                    if (dict["Status"].ToString().ToUpper() == "ALL") status = PhoneMessageStatus.All;
                                    else if (dict["Status"].ToString().ToUpper() == "RECEIVEDREAD") status = PhoneMessageStatus.ReceivedRead;
                                    else if (dict["Status"].ToString().ToUpper() == "RECEIVEDUNREAD") status = PhoneMessageStatus.ReceivedUnread;
                                    else if (dict["Status"].ToString().ToUpper() == "STOREDSENT") status = PhoneMessageStatus.StoredSent;
                                    else if (dict["Status"].ToString().ToUpper() == "STOREDUNSENT") status = PhoneMessageStatus.StoredUnsent;
                                }
                            }
                        }

                        if (dict.Keys.Contains("Storage"))
                        {
                            if (dict["Storage"] != null)
                            {
                                if (dict["Storage"] is int)
                                {
                                    if ((int)dict["Storage"] == 0) storage = PhoneStorageType.Sim;
                                    else if ((int)dict["Storage"] == 1) storage = PhoneStorageType.Phone;
                                }
                                else if (dict["Storage"] is string)
                                {
                                    if (dict["Storage"].ToString().ToUpper() == "SIM") storage = PhoneStorageType.Sim;
                                    else if (dict["Storage"].ToString().ToUpper() == "PHONE") storage = PhoneStorageType.Phone;
                                    else if(!string.IsNullOrEmpty(dict["Storage"].ToString().Trim()))storage = dict["Storage"].ToString().ToUpper();
                                }
                            }
                        }

                        var messages = comm.ReadMessages(status, storage);
                        if (messages != null) response.Add("Messages", GetList(messages));
                    }
                    else
                    {
                        ReadAllMessages();
                        response.Add("Status", "Ok");
                    }
                }
                #endregion

                #region Read Message
                else if (query == ModemQueries.ReadMessage)
                {
                    if (command != null && command is Dictionary<string, object>)
                    {
                        var dict = (Dictionary<string, object>)command;
                        var index = -1;
                        var storage = PhoneStorageType.Sim;

                        if (dict.Keys.Contains("Index") && dict["Index"] != null && dict["Index"] is int) index = (int)dict["Index"];
                        if (dict.Keys.Contains("Storage"))
                        {
                            if (dict["Storage"] != null)
                            {
                                if (dict["Storage"] is int)
                                {
                                    if ((int)dict["Storage"] == 0) storage = PhoneStorageType.Sim;
                                    else if ((int)dict["Storage"] == 1) storage = PhoneStorageType.Phone;
                                }
                                else if (dict["Storage"] is string)
                                {
                                    if (dict["Storage"].ToString().ToUpper() == "SIM") storage = PhoneStorageType.Sim;
                                    else if (dict["Storage"].ToString().ToUpper() == "PHONE") storage = PhoneStorageType.Phone;
                                }
                            }
                        }

                        if (index > -1)
                        {
                            var message = comm.ReadMessage(index, storage);
                            if (message != null)
                            {
                                SmsPdu smsrec = message.Data;
                                SmsDeliverPdu msg = (SmsDeliverPdu)smsrec;
                                //SmsStatusReportPdu data = (SmsStatusReportPdu)message.Data;
                                response.Add("Message", new Messages.ShortMessage(message.Index.ToString(), msg.UserDataText, message.Storage, msg.OriginatingAddress, msg.SmscAddress, smsrec.GetTimestamp().ToDateTime().ToString()));
                            }
                        }
                    }
                }
                #endregion

                #region Get Message Storages
                else if (query == ModemQueries.GetMessageStorages)
                {
                    var storages = comm.GetMessageStorages();
                    response.Add("ReadStorages", storages.ReadStorages.ToList());
                    response.Add("ReceiveStorages", storages.ReceiveStorages.ToList());
                    response.Add("WriteStorages", storages.WriteStorages.ToList());
                }
                #endregion

                #region Identify Device
                else if (query == ModemQueries.IdentifyDevice)
                {
                    var identity = comm.IdentifyDevice();
                    response.Add("Manufacturer", identity.Manufacturer);
                    response.Add("Model", identity.Model);
                    response.Add("Revision", identity.Revision);
                    response.Add("SerialNumber", identity.SerialNumber);

                    //if(parent != null && parent.Settings is ModuleDescription)
                    //{
                    //    var settings = (ModuleDescription)parent.Settings;
                    //    response.Add("Name", settings.Name);
                    //    response.Add("Version", settings.Version);
                    //}
                    
                }
                #endregion
            }
            return response;
        }
        #endregion 

        #region Read All Messages
        public virtual List<Message> ReadAllMessages(bool isAsync = true)
        {
            var list = new List<Message>();
            var storages = comm.GetMessageStorages();
            if (storages.ReadStorages != null)
            {
                foreach (var storage in storages.ReadStorages)
                {
                    var status = comm.GetMessageMemoryStatus(storage);
                    if (status != null && status.Used > 0) list.AddRange(ReadAllMessages(storage, isAsync));
                }
            }
            return list;
        }

        public virtual List<Message> ReadAllMessages(string storage, bool isAsync = true)
        {
            var list = new List<Message>();
            if (comm != null && comm.IsOpen())
            {
                try
                {
                    var status = PhoneMessageStatus.All;
                    DecodedShortMessage[] messages = comm.ReadMessages(status, storage);
                    if (messages != null)
                    {
                        list = GetList(messages);                      
                        if (isAsync && iEvents != null) iEvents(this, new iEventArgs(list));
                        return list;
                    }
                }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
            }
            return list;
        }
        #endregion

        #region GetList
        private List<Message> GetList(DecodedShortMessage[] messages)
        {
            var list = new List<Message>();

            foreach (var message in messages)
            {
                SmsPdu smsrec = message.Data;
                if (smsrec is SmsStatusReportPduEx)
                {
                    SmsStatusReportPduEx msg = (SmsStatusReportPduEx)smsrec;
                    var _msg = new DeliveryReport(message.Index, message.Storage, msg.SCTimestamp.ToDateTime().ToString(), msg.DischargeTime.ToDateTime().ToString(), msg.Status.Category.ToString(), msg.SmscAddress, msg.MessageReference, msg.RecipientAddress);
                    _msg.SerialNumber = GetSerial();
                    list.Add(_msg);
                }
                else if (smsrec is SmsStatusReportPdu)
                {
                    SmsStatusReportPdu msg = (SmsStatusReportPdu)smsrec;
                    var _msg = new DeliveryReport(message.Index, message.Storage, msg.SCTimestamp.ToDateTime().ToString(), msg.DischargeTime.ToDateTime().ToString(), msg.Status.Category.ToString(), msg.SmscAddress, msg.MessageReference, msg.RecipientAddress);
                    _msg.SerialNumber = GetSerial();
                    list.Add(_msg);
                }
                else if (smsrec is SmsDeliverPdu)
                {
                    SmsDeliverPdu msg = (SmsDeliverPdu)smsrec;

                    if (SmartMessageDecoder.IsPartOfConcatMessage(smsrec))
                    {
                        var info = SmartMessageDecoder.GetConcatenationInfo(smsrec);
                        var sms = new LongMessage(info.ReferenceNumber, false);
                        sms.Add(new Messages.ShortMessage(message.Index.ToString(), msg.UserDataText, message.Storage, msg.OriginatingAddress, msg.SmscAddress, smsrec.GetTimestamp().ToDateTime().ToString(), info.CurrentNumber, info.TotalMessages));
                        sms.CombineMessages();
                        sms.SerialNumber = GetSerial();
                        list.Add(sms);
                    }
                    else
                    {
                        var _msg = new Messages.ShortMessage(message.Index.ToString(), msg.UserDataText, message.Storage, msg.OriginatingAddress, msg.SmscAddress, smsrec.GetTimestamp().ToDateTime().ToString());
                        _msg.SerialNumber = GetSerial();
                        list.Add(_msg);
                    }
                }
            }
            return list;
        }
        #endregion

        #region PushNotification
        public void PushNotification(object item)
        {
            if (iEvents != null) iEvents(this, new iEventArgs(item));
        }
        #endregion
        
        #region ProcessUSSD
        private void ProcessUSSD(string ussd)
        {
            if (comm != null && comm.IsOpen())
            {
                var convertToPDU = false;
                var asPDUencoded = "\"" + ussd + "\"";
                if (convertToPDU)
                {
                    string data = TextDataConverter.StringTo7Bit(ussd);
                    asPDUencoded = Calc.IntToHex(TextDataConverter.SeptetsToOctetsInt(data));
                }

                comm.SendUssdRequest(1, asPDUencoded);
            }
        }

        public virtual string ProcessUSSD(string ussd, ref string info)
        {
            string result = "";
            string _info = "";

            if (!string.IsNullOrEmpty(ussd))
            {
                var loopEnded = false;
                var timedOut = false;
                var canTimedOut = true;
                var timedOutValue = 10;
                var startTime = DateTime.Now;

                Queue<string> steps = new Queue<string>();
                if (ussd.StartsWith("!") && ussd.EndsWith("!"))
                {
                    var _ussd = ussd.Trim(new char[] { '!' });
                    var arguments = _ussd.Split(new char[] { '|' });
                    if (arguments.Length > 0)
                    {
                        var key = arguments[0];
                        var automate = Setting.Automates.SingleOrDefault(ss => ss.key.ToLower().Equals(key.ToLower()));
                        if (automate != null)
                        {
                            ussd = automate.Command;
                            foreach (var action in automate.Actions.OrderBy(ss => ss.Key))
                                steps.Enqueue(action.Value.ToString());
                        }
                        else if (arguments.Length > 0) ussd = arguments[0].ToString();

                        if (arguments.Length > 1)
                        {
                            for (int x = 1; x < arguments.Length; x++)
                                steps.Enqueue(arguments[x].ToString());
                        }
                    }
                }

                var SmsInterfaceEventWatcher = new EventHandler<iEventArgs>((o, arg) =>
                {
                    var item = arg.Item;
                    if (item != null)
                    {
                        if (item is USSDMessage)
                        {
                            startTime = DateTime.Now;
                            canTimedOut = false;
                            timedOutValue = 20;

                            var _msg = (USSDMessage)item;
                            if (_msg.Indicator == 2 || _msg.Indicator == 0)
                            {
                                _info = "ENDED";
                                result = _msg.Message;
                                loopEnded = true;
                            }
                            else if (_msg.Indicator == 1)
                            {
                                if (steps != null && steps.Count > 0)
                                {
                                    var _ussd_option = steps.Dequeue();
                                    ProcessUSSD(_ussd_option);
                                }
                                else
                                {
                                    _info = "QUERY";
                                    result = _msg.Message;
                                    loopEnded = true;
                                }
                            }
                        }
                    }
                });

                this.iEvents += SmsInterfaceEventWatcher;

                ProcessUSSD(ussd);

                while (!loopEnded)
                {
                    TimeSpan timeout = DateTime.Now - startTime;
                    timedOut = (timeout.Seconds == timedOutValue);

                    if (timedOut && canTimedOut)
                    {
                        loopEnded = true;
                        this.iEvents -= SmsInterfaceEventWatcher;
                        this.QueryModem(ModemQueries.CancelUSSD);
                    }
                }

                if (!timedOut) this.iEvents -= SmsInterfaceEventWatcher;

                if (iEvents != null) iEvents(this, new iEventArgs(new USSDResponse(_info, result)));

                info = _info;
            }
            return result;
        }
        #endregion

        #region CancelUSSD
        public virtual void CancelUSSD()
        {
            comm.SendUssdRequest(2, "");
        }
        #endregion

        #region Read Messages From Storages
        public virtual void ReadMessagesFromStorages(List<string> storages, bool forceDelete = false)
        {
            if (!isBusy && parent != null)
            {
                try
                {
                    isBusy = true;
                    foreach (var storage in storages)
                    {
                        if (!parent.HaltModemOperations)
                        {
                            var request = new Dictionary<string, object>() { { "Status", "ALL" }, { "Storage", storage } };
                            var messages = QueryModem(ModemQueries.ReadMessages, request);
                            if (messages != null && messages.Keys.Contains("Messages"))
                            {
                                var items = (List<Message>)messages["Messages"];
                                if (items.Count > 0) Log.Trace("Read Messages From Storage: " + storage + ", Messages: " + items.Count, configuration.Verbose);
            
                                foreach (var msg in items)
                                {
                                    msg.SerialNumber = GetSerial();
                                    
                                    //var js = new JavaScriptSerializer();
                                    //js.MaxJsonLength = Int32.MaxValue;
                                    //var data = js.Serialize(msg);
                                    parent.ProcessGSM(msg);
                                    //if (parent.ProcessGSM(data))DeleteMessage(msg);
                                }
                            }
                        }
                    }
                    isBusy = false;
                }
                catch (Exception ex)
                {
                    Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose);
                    isBusy = false;
                }
            }
        }
        #endregion

        #region DeleteMessage
        public virtual bool DeleteMessage(int index, string storage, int current = 0, int retries = 10)
        {
            bool isDelete = false;
            try
            {
                if (comm != null && comm.IsOpen())
                {
                    comm.DeleteMessage(index, storage);
                    isDelete = true;
                }
            }
            catch (Exception ex)
            {
                Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose);
                if (!isDelete && current < retries)
                {
                    Thread.Sleep(10);
                    isDelete = DeleteMessage(index, storage, ++current, retries);
                }
            }
            return isDelete;
        }

        public void DeleteMessage(object item)
        {
            try
            {
                if (!isBusy && parent != null && !parent.HaltModemOperations)
                {
                    if (item is Messages.ShortMessage)
                    {
                        var msg = (Messages.ShortMessage)item;
                        var deleted = this.DeleteMessage(int.Parse(msg.Index), msg.Storage);
                        Log.Trace("ShortMessage Deleted: " + deleted + ", Message: " + msg.Message, configuration.Verbose);
                    }
                    else if (item is LongMessage)
                    {
                        var msg = (LongMessage)item;
                        this.DeleteMessage(msg);
                    }
                    else if (item is List<Message>)
                    {
                        foreach (var message in (List<Message>)item)
                        {
                            if (message != null && message is Messages.ShortMessage)
                            {
                                var msg = (Messages.ShortMessage)message;
                                this.DeleteMessage(msg);
                            }
                            else if (message != null && message is LongMessage)
                            {
                                var msg = (LongMessage)message;
                                this.DeleteMessage(msg);
                            }
                        }
                    }
                    else if (item is SentMessage)
                    {
                        var msg = (SentMessage)item;
                        this.DeleteMessage(msg);
                    }
                    else if (item is DeliveryReport)
                    {
                        var msg = (DeliveryReport)item;
                        this.DeleteMessage(msg);
                    }
                }
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
        }

        public void DeleteMessage(Message message)
        {
            try
            {
                if (!isBusy && parent != null && !parent.HaltModemOperations)
                {
                    Thread.Sleep(500);
                    
                    if (message != null && message is Messages.ShortMessage)
                    {
                        var msg = (Messages.ShortMessage)message;
                        var deleted = this.DeleteMessage(int.Parse(msg.Index), msg.Storage);
                        Log.Trace("ShortMessage Deleted: " + deleted + ", Message: " + msg.Message, configuration.Verbose);
                    }
                    else if (message != null && message is LongMessage)
                    {
                        var msgs = (LongMessage)message;
                        foreach (var msg in msgs.Messages)
                        {
                            if (!Extension.IsEmpty(msg.Index) && !Extension.IsEmpty(msg.Storage))
                            {
                                var deleted = this.DeleteMessage(int.Parse(msg.Index), msg.Storage);
                                Log.Trace("LongMessage Deleted: " + deleted + ", Message: " + msg.Message, configuration.Verbose);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
        }
        #endregion

        #region Dial
        public virtual bool Dial(string number)
        {
            return false;
        }
        #endregion

        #region HangUp
        public virtual bool HangUp()
        {
            return false;
        }
        #endregion

        #region GetSerial
        public object GetSerial()
        {
            return Setting.Parameters.FirstOrDefault(pp => !string.IsNullOrEmpty(pp.Key) && pp.Value != null && pp.Key.ToLower().Equals("deviceserialnumber")).Value;
        }
        #endregion

        #region CallForwarding
        public virtual bool CallForwarding(int reason, int mode, string number)
        {
            if (this.comm != null) return this.comm.CallForwarding(reason, mode, number);
            else return false; 
        }
        #endregion
    }
}
