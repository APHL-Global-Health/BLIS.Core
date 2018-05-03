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

using blis.Core.Configurations;
using blis.Core.Extensions;
using blis.Core.Infrastructure;
using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace blis.Linux.Extensions.V8.Helpers
{
    public class ConnectionHelper
    {
        #region Variables
        private dynamic Connection;
        private Configuration configuration;
        #endregion

        public ConnectionHelper()
        {
            configuration = Extension.GetConfiguration();
        }

        public void Open()
        {
            Connection.Open();
        }

        public void Close()
        {
            Connection.Close();
        }

        public ConnectionHelper(string connection)
            :this()
        {
            if (configuration != null && configuration.Profiles != null && !string.IsNullOrEmpty(connection))
            {
                var profile = configuration.Profiles.FirstOrDefault(ss => !string.IsNullOrEmpty(ss.Key) && ss.Key.ToLower().Equals(connection.ToLower()));
                if(profile != null)
                {
                    try
                    {
                        if (profile != null && !string.IsNullOrEmpty(profile.Engine) && profile.Value != null)
                        {
                            if (profile.Engine.Trim().ToUpper().Equals("SQL")) this.Connection = new SqlConnection(profile.Value.ToString());
                            else if (profile.Engine.Trim().ToUpper().Equals("MYSQL")) this.Connection = new MySqlConnection(profile.Value.ToString());
                            else if (profile.Engine.Trim().ToUpper().Equals("SQLITE"))
                            {
                                var _connectionString = "";
                                var parts = profile.Value.ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
                                foreach (var part in parts)
                                {
                                    var sections = part.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                                    if (sections.Count == 2)
                                    {
                                        var key = sections[0];
                                        var value = sections[1];
                                        if (key.ToLower() == "data source")
                                        {
                                            var dbPath = Environment.CurrentDirectory + "\\" + value;
                                            var dir = Path.GetDirectoryName(dbPath);
                                            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                                            if (!File.Exists(dbPath)) SQLiteConnection.CreateFile(dbPath);
                                            _connectionString += key + "=" + dbPath + ";";
                                        }
                                        else _connectionString += key + "=" + value + ";";
                                    }
                                }
                                if (!string.IsNullOrEmpty(_connectionString)) this.Connection = new SQLiteConnection(_connectionString);
                            }
                        }
                    }
                    catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace); }
                }
            }
        }

        public ConnectionHelper(SqlConnection connection)
        {
            this.Connection = connection;
        }

        public ConnectionHelper(MySqlConnection connection)
        {
            this.Connection = connection;
        }

        public ConnectionHelper(SQLiteConnection connection)
        {
            this.Connection = connection;
        }

        public CommandHelper CreateCommand(string sql)
        {
            try
            {
                if (this.Connection != null)
                {
                    if (this.Connection is SqlConnection) return new CommandHelper(new SqlCommand(sql, this.Connection) { CommandTimeout = 0 });
                    else if (this.Connection is MySqlConnection) return new CommandHelper(new MySqlCommand(sql, this.Connection) { CommandTimeout = 0 });
                    else if (this.Connection is SQLiteConnection) return new CommandHelper(new SQLiteCommand(sql, this.Connection) { CommandTimeout = 0 });
                }
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace); }

            return null;
        }
    }
}
