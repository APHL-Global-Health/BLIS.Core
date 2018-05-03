// --------------------------------------------------------------------------------------------------------------------
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
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// This is a port from CefGlue.WindowsForms sample of CefGlue. Mostly provided as-is. 
// For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
// </note>
// --------------------------------------------------------------------------------------------------------------------

using blis.Core.Infrastructure;
using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace blis.Windows.Extensions.V8.Helpers
{
    public class DataReaderHelper
    {
        private dynamic Reader;

        public DataReaderHelper(SqlDataReader reader)
        {
            Reader = reader;
        }

        public DataReaderHelper(MySqlDataReader reader)
        {
            Reader = reader;
        }

        public DataReaderHelper(SQLiteDataReader reader)
        {
            Reader = reader;
        }
        
        public bool Read()
        {
            try { if (Reader != null) return Reader.Read(); }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
            return false;
        }

        public int GetOrdinal(string name)
        {
            try
            {
                if (Reader != null)
                {
                    var ordinal = Reader.GetOrdinal(name);
                    return ordinal;
                }
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
            return -1;
        }

        public object GetValue(int i)
        {
            try
            {
                if (Reader != null)
                {
                    if (i > -1)
                    {
                        if (!Reader.IsDBNull(i))
                        {
                            var value = Reader.GetValue(i);
                            if (value is DateTime) return value.ToString();
                            else return value;
                        }
                    }
                }                
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
            return null;
        }

        public void Close()
        {
            try { if (Reader != null) Reader.Close(); }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
        }
        
    }
}
