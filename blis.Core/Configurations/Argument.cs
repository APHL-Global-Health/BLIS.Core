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
using System.Xml.Serialization;

namespace blis.Core.Configurations
{
    public class Argument
    {
        public Argument()
        {
            Parameters = new List<Parameter>();
            Dependencies = new List<Dependency>();
            Formats = new List<Format>();
            Automates = new List<Automate>();
        }

        public Argument(string Name, string Version, bool Ignore, string Type)
        {
            this.Name = Name;
            this.Version = Version;
            this.Ignore = Ignore;
            this.Type = Type;
        }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Version { get; set; }

        [XmlArray]
        public List<Dependency> Dependencies { get; set; }

        [XmlArray]
        public List<Format> Formats { get; set; }

        [XmlArray]
        public List<Parameter> Parameters { get; set; }

        [XmlArray]
        public List<Automate> Automates { get; set; }

        [XmlAttribute]
        public bool Ignore { get; set; }

        [XmlAttribute]
        public string Type { get; set; }

        public Argument AddParameter(string Key, object Value)
        {
            lock (Parameters) this.Parameters.Add(new Parameter(Key, Value));
            return this;
        }

        public Argument AddAutomation(string Key, string Command)
        {
            lock (Automates) this.Automates.Add(new Automate(Key, Command));
            return this;
        }

        public Argument AddFormat(string Key, string Rule, object Value)
        {
            lock (Formats) this.Formats.Add(new Format(Key, Rule, Value));
            return this;
        }

        public Argument AddDependency(string Assemby)
        {
            lock (Dependencies) this.Dependencies.Add(new Dependency(Assemby));
            return this;
        }
    }
}
