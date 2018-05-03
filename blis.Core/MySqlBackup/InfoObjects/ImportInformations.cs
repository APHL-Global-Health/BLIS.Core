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
    public class ImportInformations
    {
        int _interval = 100;
        string _targetDatabase = "";
        string _databaseDefaultCharSet = "";

        /// <summary>
        /// Gets or Sets a value indicates whether the Imported Dump File is encrypted.
        /// </summary>
        [System.Obsolete("This implementation will slow down the whole process which is not recommended. Encrypt the content externally after the export process completed. For more information, please read documentation.")]
        public bool EnableEncryption = false;

        /// <summary>
        /// Sets the password used to decrypt the exported dump file.
        /// </summary>
        [System.Obsolete("This implementation will slow down the whole process which is not recommended. Encrypt the content externally after the export process completed. For more information, please read documentation.")]
        public string EncryptionPassword = "";

        /// <summary>
        /// Gets or Sets a value indicates the interval of time (in miliseconds) to raise the event of ExportProgressChanged.
        /// </summary>
        public int IntervalForProgressReport { get { if (_interval == 0) return 100; return _interval; } set { _interval = value; } }

        /// <summary>
        /// Gets or Sets the name of target database.
        /// </summary>
        public string TargetDatabase { get { return (_targetDatabase + "").Trim(); } set { _targetDatabase = value; } }

        /// <summary>
        /// Gets or Sets the default character set of the target database. This will only take effect when targetting new non-existed database.
        /// </summary>
        public string DatabaseDefaultCharSet { get { return (_databaseDefaultCharSet + "").Trim(); } set { _databaseDefaultCharSet = value; } }

        /// <summary>
        /// Gets or Sets a value indicates whether SQL errors occurs in import process should be ignored.
        /// </summary>
        public bool IgnoreSqlError = false;

        /// <summary>
        /// Gets or Sets the file path used to log error messages.
        /// </summary>
        public string ErrorLogFile = "";

        public ImportInformations()
        { }
    }
}
