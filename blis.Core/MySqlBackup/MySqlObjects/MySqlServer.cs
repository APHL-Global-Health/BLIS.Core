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
// blis project is licensed under MIT License. MySqlBackup may have additional licensing.
// This is a port from CefGlue.WindowsForms sample of CefGlue. Mostly provided as-is. 
// For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
// </note>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.Data.MySqlClient
{
    public class MySqlServer
    {
        string _versionNumber;
        string _edition;
        decimal _majorVersionNumber = 0;
        string _characterSetServer = "";
        string _characterSetSystem = "";
        string _characterSetConnection = "";
        string _characterSetDatabase = "";
        string _currentUser = "";
        string _currentUserClientHost = "";
        string _currentClientHost = "";

        public string Version { get { return string.Format("{0} {1}", _versionNumber, _edition); } }
        public string VersionNumber { get { return _versionNumber; } }
        public decimal MajorVersionNumber { get { return _majorVersionNumber; } }
        public string Edition { get { return _edition; } }
        public string CharacterSetServer { get { return _characterSetServer; } }
        public string CharacterSetSystem { get { return _characterSetSystem; } }
        public string CharacterSetConnection { get { return _characterSetConnection; } }
        public string CharacterSetDatabase { get { return _characterSetDatabase; } }
        public string CurrentUser { get { return _currentUser; } }
        public string CurrentUserClientHost { get { return _currentUserClientHost; } }
        public string CurrentClientHost { get { return _currentClientHost; } }

        public MySqlServer()
        { }

        public void GetServerInfo(MySqlCommand cmd)
        {
            _edition = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'version_comment';", 1);
            _versionNumber = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'version';", 1);
            _characterSetServer = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'character_set_server';", 1);
            _characterSetSystem = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'character_set_system';", 1);
            _characterSetConnection = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'character_set_connection';", 1);
            _characterSetDatabase = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'character_set_database';", 1);

            _currentUserClientHost = QueryExpress.ExecuteScalarStr(cmd, "SELECT current_user;");

            string[] ca = _currentUserClientHost.Split('@');

            _currentUser = ca[0];
            _currentClientHost = ca[1];

            GetMajorVersionNumber();
        }

        void GetMajorVersionNumber()
        {
            string[] vsa = _versionNumber.Split('.');
            string v = "";
            if (vsa.Length > 1)
                v = vsa[0] + "." + vsa[1];
            else
                v = vsa[0];
            decimal.TryParse(v, out _majorVersionNumber);
        }
    }
}

