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
    public class MySqlTrigger
    {
        string _name = "";
        string _createTriggerSQL = "";
        string _createTriggerSQLWithoutDefiner = "";

        public string Name { get { return _name; } }
        public string CreateTriggerSQL { get { return _createTriggerSQL; } }
        public string CreateTriggerSQLWithoutDefiner { get { return _createTriggerSQLWithoutDefiner; } }

        public MySqlTrigger(MySqlCommand cmd, string triggerName, string definer)
        {
            _name = triggerName;

            _createTriggerSQL = QueryExpress.ExecuteScalarStr(cmd, string.Format("SHOW CREATE TRIGGER `{0}`;", triggerName), 2);

            _createTriggerSQL = _createTriggerSQL.Replace("\r\n", "^~~~~~~~~~~~~~~^");
            _createTriggerSQL = _createTriggerSQL.Replace("\n", "^~~~~~~~~~~~~~~^");
            _createTriggerSQL = _createTriggerSQL.Replace("\r", "^~~~~~~~~~~~~~~^");
            _createTriggerSQL = _createTriggerSQL.Replace("^~~~~~~~~~~~~~~^", "\r\n");

            string[] sa = definer.Split('@');
            definer = string.Format(" DEFINER=`{0}`@`{1}`", sa[0], sa[1]);

            _createTriggerSQLWithoutDefiner = _createTriggerSQL.Replace(definer, string.Empty);
        }
    }
}
