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
    public class MySqlColumnList : IDisposable
    {
        string _tableName;
        List<MySqlColumn> _lst = new List<MySqlColumn>();
        string _sqlShowFullColumns = "";

        public string SqlShowFullColumns { get { return _sqlShowFullColumns; } }

        public MySqlColumnList()
        { }

        public MySqlColumnList(MySqlCommand cmd, string tableName)
        {
            _tableName = tableName;
            DataTable dtDataType = QueryExpress.GetTable(cmd, string.Format("SELECT * FROM  `{0}` where 1 = 2;", tableName));
            
            _sqlShowFullColumns = string.Format("SHOW FULL COLUMNS FROM `{0}`;", tableName);
            DataTable dtColInfo = QueryExpress.GetTable(cmd, _sqlShowFullColumns);

            for (int i = 0; i < dtDataType.Columns.Count; i++)
            {
                string isNullStr = (dtColInfo.Rows[i]["Null"] + "").ToLower();
                bool isNull = false;
                if (isNullStr == "yes")
                    isNull = true;

                _lst.Add(new MySqlColumn(
                    dtDataType.Columns[i].ColumnName, 
                    dtDataType.Columns[i].DataType,
                    dtColInfo.Rows[i]["Type"] + "", 
                    dtColInfo.Rows[i]["Collation"] + "",
                    isNull, 
                    dtColInfo.Rows[i]["Key"] + "",
                    dtColInfo.Rows[i]["Default"] + "", 
                    dtColInfo.Rows[i]["Extra"] + "",
                    dtColInfo.Rows[i]["Privileges"] + "", 
                    dtColInfo.Rows[i]["Comment"] + ""));
            }
        }

        public MySqlColumn this[int columnIndex]
        {
            get
            {
                return _lst[columnIndex];
            }
        }

        public MySqlColumn this[string columnName]
        {
            get
            {
                for (int i = 0; i < _lst.Count; i++)
                {
                    if (_lst[i].Name == columnName)
                    {
                        return _lst[i];
                    }
                }
                throw new Exception("Column \"" + columnName + "\" is not existed in table \"" + _tableName + "\".");
            }
        }

        public int Count
        {
            get
            {
                return _lst.Count;
            }
        }

        public bool Contains(string columnName)
        {
            if (this[columnName] == null)
                return false;
            return true;
        }

        public void Dispose()
        {
            for (int i = 0; i < _lst.Count; i++)
            {
                _lst[i] = null;
            }
            _lst = null;
        }

        public IEnumerator<MySqlColumn> GetEnumerator()
        {
            return _lst.GetEnumerator();
        }

    }
}
