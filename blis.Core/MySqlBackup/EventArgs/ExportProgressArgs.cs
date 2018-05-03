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
    public class ExportProgressArgs : EventArgs
    {
        string _currentTableName = "";
        long _totalRowsInCurrentTable = 0;
        long _totalRowsInAllTables = 0;
        long _currentRowIndexInCurrentTable = 0;
        long _currentRowIndexInAllTables = 0;
        int _totalTables = 0;
        int _currentTableIndex = 0;

        public string CurrentTableName { get { return _currentTableName; } }
        public long TotalRowsInCurrentTable { get { return _totalRowsInCurrentTable; } }
        public long TotalRowsInAllTables { get { return _totalRowsInAllTables; } }
        public long CurrentRowIndexInCurrentTable { get { return _currentRowIndexInCurrentTable; } }
        public long CurrentRowIndexInAllTables { get { return _currentRowIndexInAllTables; } }
        public int TotalTables { get { return _totalTables; } }
        public int CurrentTableIndex { get { return _currentTableIndex; } }

        public ExportProgressArgs(string currentTableName,
            long totalRowsInCurrentTable,
            long totalRowsInAllTables,
            long currentRowIndexInCurrentTable,
            long currentRowIndexInAllTable,
            int totalTables,
            int currentTableIndex)
        {
            _currentTableName = currentTableName;
            _totalRowsInCurrentTable = totalRowsInCurrentTable;
            _totalRowsInAllTables = totalRowsInAllTables;
            _currentRowIndexInCurrentTable = currentRowIndexInCurrentTable;
            _currentRowIndexInAllTables = currentRowIndexInAllTable;
            _totalTables = totalTables;
            _currentTableIndex = currentTableIndex;
        }
    }
}
