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
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace blis.Core.Configurations
{
    public class Configuration
    {
        [XmlAttribute]
        public string Application { get; set; }

        [XmlAttribute]
        public int RemoteDebuggingPort { get; set; } 

        [XmlAttribute]
        public int SessionLifetime { get; set; }

        [XmlAttribute]
        public bool Verbose { get; set; }

        private static object _lock = new object();
        public List<Profile> Profiles { get; private set; }
        public List<Argument> Arguments { get; private set; }

        public Configuration()
        {
            this.Profiles = new List<Profile>();
            this.Arguments = new List<Argument>();
        }

        public Configuration(string Application, int SessionLifetime, bool Verbose, int RemoteDebuggingPort = 20480)
            :this()
        {
            this.Application = Application;
            this.SessionLifetime = SessionLifetime;
            this.Verbose = Verbose;
            this.RemoteDebuggingPort = RemoteDebuggingPort;
        }

        public Configuration AddProfile(string Key, string Engine, object Value)
        {
            lock (Profiles) this.Profiles.Add(new Profile(Key, Engine, Value));
            return this;
        }

        public Configuration AddArgument(string Key, string Engine, object Value)
        {
            lock (Profiles) this.Profiles.Add(new Profile(Key, Engine, Value));
            return this;
        }
        
        public static Configuration Load(string filePath = "Configurations.xml")
        {
            lock (_lock)
            {
                if (string.IsNullOrEmpty(filePath)) return null;

                var reader = new XmlSerializer(typeof(Configuration));
                using (var file = new StreamReader(filePath))
                {
                    return (Configuration)reader.Deserialize(file);
                }
            }
        }

        public Configuration Save(string filePath = "Configurations.xml")
        {
            lock (_lock)
            {
                var serializer = new XmlSerializer(typeof(Configuration));
                using (TextWriter UpdateConfigurationFileStream = new StreamWriter(filePath))
                {
                    serializer.Serialize(UpdateConfigurationFileStream, this);
                }
            }

            return this;
        }

        #region WebStartupFile
        public static string WebStartupFile()
        {
            var html = " <!DOCTYPE HTML>" + Environment.NewLine;
            html += " <html>" + Environment.NewLine;
            html += " 	<head>" + Environment.NewLine;
            html += " 		<meta name='author' content='Fredrick Mwasekaga' />" + Environment.NewLine;
            html += " 			<meta name='description' content='Kaga Connect is a software+services company' />" + Environment.NewLine;
            html += " 			<meta name='keywords' content='kaga,connect' />" + Environment.NewLine;
            html += " 			<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/>" + Environment.NewLine;
            html += " 			<meta name='viewport' content='width=device-width, initial-scale=1'>" + Environment.NewLine;
            html += " 			<title>blis</title>	" + Environment.NewLine;
            html += " 	</head>" + Environment.NewLine;
            html += " 	<body>" + Environment.NewLine;
            html += " 	</body>" + Environment.NewLine;
            html += " </html>";
            return html;
        }
        #endregion
    }
}
