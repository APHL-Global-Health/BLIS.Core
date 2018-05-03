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

#region Using
using blis.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Management;
using System.Text.RegularExpressions;
#endregion

namespace blis.Core.USB
{
    public class DeviceInfo
    {
        #region Properties
        public string Availability { get; set; }
        public string Caption { get; set; }
        public string ClassGuid { get; set; }
        public string[] CompatibleID { get; set; }
        public int ConfigManagerErrorCode { get; set; }
        public bool ConfigManagerUserConfig { get; set; }
        public string CreationClassName { get; set; }
        public string Description { get; set; }
        public string DeviceID { get; set; }
        public string ErrorCleared { get; set; }
        public string ErrorDescription { get; set; }
        public string[] HardwareID { get; set; }
        public string InstallDate { get; set; }
        public string LastErrorCode { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public string PNPDeviceID { get; set; }
        public string PowerManagementCapabilities { get; set; }
        public string PowerManagementSupported { get; set; }
        public string Service { get; set; }
        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public string SystemCreationClassName { get; set; }
        public string SystemName { get; set; }


        public string PortName { get; set; }
        public string GenericID { get; set; }
        public string SpecificID { get; set; }
        public string OtherID { get; set; }

        public string PID { get; set; }
        public string VID { get; set; }
        public string MI { get; set; }
        public string REV { get; set; }
        #endregion

        #region GetDevices
        public static List<DeviceInfo> GetDevices(string where = "")
        {
            var list = new List<DeviceInfo>();
            using (var searcher = new ManagementObjectSearcher("Select * From Win32_PnPEntity " + where))
            {
                using (ManagementObjectCollection collection = searcher.Get())
                {
                    foreach (var device in collection)
                    {
                        var dev = new DeviceInfo();
                        foreach (var property in device.Properties)
                        {
                            if (property.Name.ToLower().Equals("availability")) dev.Availability = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("caption")) dev.Caption = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("classguid")) dev.ClassGuid = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("compatibleid")) dev.CompatibleID = (string[])property.Value;
                            else if (property.Name.ToLower().Equals("configmanagererrorcode")) dev.ConfigManagerErrorCode = (property.Value.IsNumber() ? int.Parse(property.Value.ToString()) : 0);
                            else if (property.Name.ToLower().Equals("configmanageruserconfig")) dev.ConfigManagerUserConfig = (bool)property.Value;
                            else if (property.Name.ToLower().Equals("creationclassname")) dev.CreationClassName = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("description")) dev.Description = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("errorcleared")) dev.ErrorCleared = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("errordescription")) dev.ErrorDescription = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("hardwareid")) dev.HardwareID = (string[])property.Value;
                            else if (property.Name.ToLower().Equals("installdate")) dev.InstallDate = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("lasterrorcode")) dev.LastErrorCode = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("manufacturer")) dev.Manufacturer = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("pnpdeviceid")) dev.PNPDeviceID = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("powermanagementcapabilities")) dev.PowerManagementCapabilities = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("powermanagementsupported")) dev.PowerManagementSupported = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("service")) dev.Service = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("status")) dev.Status = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("statusinfo")) dev.StatusInfo = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("systemcreationclassname")) dev.SystemCreationClassName = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("systemname")) dev.SystemName = Extension.IfNullMakeEmpty(property.Value);
                            else if (property.Name.ToLower().Equals("deviceid"))
                            {
                                dev.DeviceID = Extension.IfNullMakeEmpty(property.Value);
                                if (!string.IsNullOrEmpty(dev.DeviceID))
                                {
                                    var ids = dev.DeviceID.Split(new char[] { '\\' }, StringSplitOptions.None);
                                    if (ids.Length > 0)
                                    {
                                        dev.GenericID = ids[0];
                                        if (ids.Length > 1)
                                        {
                                            dev.SpecificID = ids[1];
                                            if (!string.IsNullOrEmpty(dev.SpecificID))
                                            {
                                                foreach (var item in dev.SpecificID.Split(new char[] { '&' }, StringSplitOptions.None))
                                                {
                                                    if (item.Contains("PID_")) dev.PID = item.Replace("PID_", "");
                                                    else if (item.Contains("VID_")) dev.VID = item.Replace("VID_", "");
                                                    else if (item.Contains("MI_")) dev.MI = item.Replace("MI_", "");
                                                    else if (item.Contains("REV_")) dev.REV = item.Replace("REV_", "");
                                                }
                                            }
                                            if (ids.Length > 2) dev.OtherID = ids[2];
                                        }
                                    }
                                }
                            }
                            else if (property.Name.ToLower().Equals("name"))
                            {
                                dev.Name = Extension.IfNullMakeEmpty(property.Value);
                                if (!string.IsNullOrEmpty(dev.Name))
                                {
                                    var m = Regex.Match(dev.Name, "(.+)(COM([0-9]+))");
                                    if (m.Success) dev.PortName = m.Groups[3].Value;
                                }
                            }
                        }
            
            
                        list.Add(dev);
                    }
                }
            }
            return list;
        }
        #endregion
    }
}
