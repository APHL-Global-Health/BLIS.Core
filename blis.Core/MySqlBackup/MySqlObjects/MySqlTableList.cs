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
using System.Data;

namespace MySql.Data.MySqlClient
{
    public class MySqlTableList : IDisposable
    {
        List<MySqlTable> _lst = new List<MySqlTable>();
        string _sqlShowFullTables = "";

        public string SqlShowFullTables { get { return _sqlShowFullTables; } }

        public MySqlTableList()
        { }

        public MySqlTableList(MySqlCommand cmd)
        {
            _sqlShowFullTables = "SHOW FULL TABLES WHERE Table_type = 'BASE TABLE';";
            DataTable dtTableList = QueryExpress.GetTable(cmd, _sqlShowFullTables);

            foreach (DataRow dr in dtTableList.Rows)
            {
                _lst.Add(new MySqlTable(cmd, dr[0] + ""));
            }
        }

        public MySqlTable this[int tableIndex]
        {
            get 
            {
                return _lst[tableIndex];
            }
        }

        public MySqlTable this[string tableName]
        {
            get
            {
                for (int i = 0; i < _lst.Count; i++)
                {
                    if (_lst[i].Name == tableName)
                        return _lst[i];
                }
                throw new Exception("Table \"" + tableName + "\" is not existed.");
            }
        }

        public int Count
        {
            get
            {
                return _lst.Count;
            }
        }

        public IEnumerator<MySqlTable> GetEnumerator()
        {
            return _lst.GetEnumerator();
        }

        public void Dispose()
        {
            for (int i = 0; i < _lst.Count; i++)
            {
                _lst[i].Dispose();
                _lst[i] = null;
            }
            _lst = null;
        }
    }
}
