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

using System.Collections.Generic;

namespace blis.Core.Helpers
{
    public class DeviceClasses
    {
        public string Class { get; private set; }
        public string GUID { get; private set; }
        public string Description { get; private set; }

        public DeviceClasses(string Class, string GUID, string Description)
        {
            this.Class = Class;
            this.GUID = GUID;
            this.Description = Description;
        }

        public static List<DeviceClasses> Devices()
        {
            var list = new List<DeviceClasses>();
            list.Add(new DeviceClasses("CDROM", "4D36E965-E325-11CE-BFC1-08002BE10318", "CD/DVD/Blu-ray drives"));
            list.Add(new DeviceClasses("DiskDrive", "4D36E967-E325-11CE-BFC1-08002BE10318", "Hard drives"));
            list.Add(new DeviceClasses("Display", "4D36E968-E325-11CE-BFC1-08002BE10318", "Video adapters"));
            list.Add(new DeviceClasses("FDC", "4D36E969-E325-11CE-BFC1-08002BE10318", "Floppy controllers"));
            list.Add(new DeviceClasses("FloppyDisk", "4D36E980-E325-11CE-BFC1-08002BE10318", "Floppy drives"));
            list.Add(new DeviceClasses("HDC", "4D36E96A-E325-11CE-BFC1-08002BE10318", "Hard drive controllers"));
            list.Add(new DeviceClasses("HIDClass", "745A17A0-74D3-11D0-B6FE-00A0C90F57DA", "Some USB devices"));
            list.Add(new DeviceClasses("1394", "6BDD1FC1-810F-11D0-BEC7-08002BE2092F", "IEEE 1394 host controller"));
            list.Add(new DeviceClasses("Image", "6BDD1FC6-810F-11D0-BEC7-08002BE2092F", "Cameras and scanners"));
            list.Add(new DeviceClasses("Keyboard", "4D36E96B-E325-11CE-BFC1-08002BE10318", "Keyboards"));
            list.Add(new DeviceClasses("Modem", "4D36E96D-E325-11CE-BFC1-08002BE10318", "Modems"));
            list.Add(new DeviceClasses("Mouse", "4D36E96F-E325-11CE-BFC1-08002BE10318", "Mice and pointing devices"));
            list.Add(new DeviceClasses("Media", "4D36E96C-E325-11CE-BFC1-08002BE10318", "Audio and video devices"));
            list.Add(new DeviceClasses("Net", "4D36E972-E325-11CE-BFC1-08002BE10318", "Network adapters"));
            list.Add(new DeviceClasses("Ports", "4D36E978-E325-11CE-BFC1-08002BE10318", "Serial and parallel ports"));
            list.Add(new DeviceClasses("SCSIAdapter", "4D36E97B-E325-11CE-BFC1-08002BE10318", "SCSI and RAID controllers"));
            list.Add(new DeviceClasses("System", "4D36E97D-E325-11CE-BFC1-08002BE10318", "System buses, bridges, etc."));
            list.Add(new DeviceClasses("USB", "36FC9E60-C465-11CF-8056-444553540000", "USB host controllers and hubs"));

            list.Add(new DeviceClasses("LibUSB", "eb781aaf-9c70-4523-a5df-642a87eca567", "Lib USB"));
            return list;
        }
    }
}
