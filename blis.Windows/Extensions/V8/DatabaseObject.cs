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

#region Using
using blis.Core.Configurations;
using blis.Core.Extensions.V8.Helpers;
using blis.Core.Infrastructure;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using V8.Net;
#endregion

namespace blis.Windows.Extensions.V8
{
    public class DatabaseObject : V8NativeObject
    {
        #region Variables
        private static Configuration configuration;
        #endregion

        #region Initialize
        public override InternalHandle Initialize(bool isConstructCall, params InternalHandle[] args)
        {
            var path = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            if (File.Exists(Path.Combine(path, "Configurations.xml")))
                configuration = Configuration.Load(Path.Combine(path, "Configurations.xml"));

            this.AsDynamic.CreateConnection = Engine.CreateFunctionTemplate().GetFunctionObject(CreateConnection);
            this.AsDynamic.SQLString_DatabaseInfomation = Engine.CreateFunctionTemplate().GetFunctionObject(SQLString_DatabaseInfomation);
            this.AsDynamic.GetDatabaseInfomation = Engine.CreateFunctionTemplate().GetFunctionObject(GetDatabaseInfomation);
            
            return base.Initialize(isConstructCall, args);
        }
        #endregion

        #region Create Connection
        public InternalHandle CreateConnection(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            try
            {
                if (args.Length >= 1)
                {
                    var profileName = args[0].AsString;
                    InternalHandle callback = (args.Length == 2 ? args[1].KeepAlive() : null);

                    if (!string.IsNullOrEmpty(profileName))
                    {
                        if (callback != null && callback.IsFunction)
                        {
                            new Task(() =>
                            {

                                try { callback.StaticCall(Engine.CreateValue(new ConnectionHelper(profileName))); }
                                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }

                            }).Start();
                        }
                        else
                        {
                            try { return Engine.CreateValue(new ConnectionHelper(profileName)); }
                            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }

            return null;
        }
        #endregion
        
        #region SQLString Database Infomation
        public InternalHandle SQLString_DatabaseInfomation(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            try
            {
                if (args.Length >= 1)
                {
                    if (args[0] != null && args[0].Value != null)
                    {
                        if (args[0].IsString) return Engine.CreateValue(SQLString_DatabaseInfomation(args[0].AsString));
                        else if (args[0].Value is Profile) return Engine.CreateValue(SQLString_DatabaseInfomation((Profile)(args[0].Value)));
                    }
                }
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }

            return null;
        }

        public static string SQLString_DatabaseInfomation(string name)
        {
            if (configuration != null && configuration.Profiles != null && !string.IsNullOrEmpty(name))
                return SQLString_DatabaseInfomation(configuration.Profiles.FirstOrDefault(ss => !string.IsNullOrEmpty(ss.Key) && ss.Key.ToLower().Equals(name.ToLower())));
            return null;
        }

        public static string SQLString_DatabaseInfomation(Profile profile)
        {
            if (profile != null && !string.IsNullOrEmpty(profile.Engine) && profile.Value != null)
            {
                if (profile.Engine.Trim().ToUpper().Equals("SQL")) return "SELECT [TABLE_CATALOG], [TABLE_NAME],[COLUMN_NAME],[ORDINAL_POSITION],[COLUMN_DEFAULT],[IS_NULLABLE],[DATA_TYPE],[CHARACTER_MAXIMUM_LENGTH] FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_CATALOG = DB_NAME() order by [TABLE_NAME],[ORDINAL_POSITION]";
                else if (profile.Engine.Trim().ToUpper().Equals("MYSQL")) return "select `TABLE_SCHEMA`, `TABLE_NAME`, `COLUMN_NAME`, `ORDINAL_POSITION`, `COLUMN_DEFAULT`, `IS_NULLABLE`, `DATA_TYPE`, `CHARACTER_MAXIMUM_LENGTH` from information_schema.columns where TABLE_SCHEMA = DATABASE() order by table_name,ordinal_position";
                else if (profile.Engine.Trim().ToUpper().Equals("SQLITE")) return "SELECT type, name, tbl_name, rootpage, sql FROM sqlite_master WHERE type='table' ORDER BY name";
            }
            return null;
        }
        #endregion

        #region Get Database Infomation
        public InternalHandle GetDatabaseInfomation(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            try
            {
                if (args.Length >= 1)
                {
                    if (args[1] != null && args[1].Value != null && args[2] != null && args[2].Value != null)
                    {
                        if (args[0].IsString)
                        {
                            if (args[1].Value is MySqlDataReader && args[2].Value is MySqlConnection) return Engine.CreateValue(GetDatabaseInfomation(args[0].AsString, (MySqlDataReader)(args[1].Value), (MySqlConnection)(args[2].Value)));
                            else if (args[1].Value is SqlDataReader && args[2].Value is SqlConnection) return Engine.CreateValue(GetDatabaseInfomation(args[0].AsString, (SqlDataReader)(args[1].Value), (SqlConnection)(args[2].Value)));
                            else if (args[1].Value is SQLiteDataReader && args[2].Value is SQLiteConnection) return Engine.CreateValue(GetDatabaseInfomation(args[0].AsString, (SQLiteDataReader)(args[1].Value), (SQLiteConnection)(args[2].Value)));
                        }
                        else if (args[0].Value is Profile)
                        {
                            if (args[1].Value is MySqlDataReader && args[2].Value is MySqlConnection) return Engine.CreateValue(GetDatabaseInfomation((Profile)(args[0].Value), (MySqlDataReader)(args[1].Value), (MySqlConnection)(args[2].Value)));
                            else if (args[1].Value is SqlDataReader && args[2].Value is SqlConnection) return Engine.CreateValue(GetDatabaseInfomation((Profile)(args[0].Value), (SqlDataReader)(args[1].Value), (SqlConnection)(args[2].Value)));
                            else if (args[1].Value is SQLiteDataReader && args[2].Value is SQLiteConnection) return Engine.CreateValue(GetDatabaseInfomation((Profile)(args[0].Value), (SQLiteDataReader)(args[1].Value), (SQLiteConnection)(args[2].Value)));
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }

            return null;
        }

        public static List<Dictionary<string, object>> GetDatabaseInfomation(string name, MySqlDataReader dataReader, MySqlConnection connection)
        {
            if (configuration != null && configuration.Profiles != null && !string.IsNullOrEmpty(name))
                return GetDatabaseInfomation(configuration.Profiles.FirstOrDefault(ss => !string.IsNullOrEmpty(ss.Key) && ss.Key.ToLower().Equals(name.ToLower())), dataReader, connection);
            return null;
        }

        public static List<Dictionary<string, object>> GetDatabaseInfomation(string name, SqlDataReader dataReader, SqlConnection connection)
        {
            if (configuration != null && configuration.Profiles != null && !string.IsNullOrEmpty(name))
                return GetDatabaseInfomation(configuration.Profiles.FirstOrDefault(ss => !string.IsNullOrEmpty(ss.Key) && ss.Key.ToLower().Equals(name.ToLower())), dataReader, connection);
            return null;
        }

        public static List<Dictionary<string, object>> GetDatabaseInfomation(string name, SQLiteDataReader dataReader, SQLiteConnection connection)
        {
            if (configuration != null && configuration.Profiles != null && !string.IsNullOrEmpty(name))
                return GetDatabaseInfomation(configuration.Profiles.FirstOrDefault(ss => !string.IsNullOrEmpty(ss.Key) && ss.Key.ToLower().Equals(name.ToLower())), dataReader, connection);
            return null;
        }


        public static List<Dictionary<string, object>> GetDatabaseInfomation(Profile profile, MySqlDataReader dataReader, MySqlConnection connection)
        {
            var list = new List<Dictionary<string, object>>();
            if (profile != null && !string.IsNullOrEmpty(profile.Engine) && profile.Value != null)
            {
                if (profile.Engine.Trim().ToUpper().Equals("MYSQL"))
                {
                    while (dataReader.Read())
                    {
                        var database = dataReader["TABLE_SCHEMA"];
                        var table = dataReader["TABLE_NAME"];
                        var column = dataReader["COLUMN_NAME"];
                        var ORDINAL_POSITION = dataReader["ORDINAL_POSITION"];
                        var IS_NULLABLE = dataReader["IS_NULLABLE"];
                        var COLUMN_DEFAULT = dataReader["COLUMN_DEFAULT"];
                        var DATA_TYPE = dataReader["DATA_TYPE"];
                        var CHARACTER_MAXIMUM_LENGTH = dataReader["CHARACTER_MAXIMUM_LENGTH"];

                        var dict = new Dictionary<string, object>();
                        dict.Add("DATABASE", database);
                        dict.Add("TABLE", table);
                        dict.Add("COLUMN", column);
                        dict.Add("DATA_TYPE", DATA_TYPE);
                        dict.Add("IS_NULLABLE", IS_NULLABLE);
                        dict.Add("COLUMN_DEFAULT", COLUMN_DEFAULT);
                        dict.Add("ORDINAL_POSITION", ORDINAL_POSITION);
                        dict.Add("CHARACTER_MAXIMUM_LENGTH", CHARACTER_MAXIMUM_LENGTH);
                        list.Add(dict);
                    }
                }
            }
            return list;
        }

        public static List<Dictionary<string, object>> GetDatabaseInfomation(Profile profile, SqlDataReader dataReader, SqlConnection connection)
        {
            var list = new List<Dictionary<string, object>>();
            if (profile != null && !string.IsNullOrEmpty(profile.Engine) && profile.Value != null)
            {
                if (profile.Engine.Trim().ToUpper().Equals("SQL"))
                {

                    while (dataReader.Read())
                    {
                        var database = dataReader["TABLE_CATALOG"];
                        var table = dataReader["TABLE_NAME"];
                        var column = dataReader["COLUMN_NAME"];
                        var ORDINAL_POSITION = dataReader["ORDINAL_POSITION"];
                        var IS_NULLABLE = dataReader["IS_NULLABLE"];
                        var COLUMN_DEFAULT = dataReader["COLUMN_DEFAULT"];
                        var DATA_TYPE = dataReader["DATA_TYPE"];
                        var CHARACTER_MAXIMUM_LENGTH = dataReader["CHARACTER_MAXIMUM_LENGTH"];

                        var dict = new Dictionary<string, object>();
                        dict.Add("DATABASE", database);
                        dict.Add("TABLE", table);
                        dict.Add("COLUMN", column);
                        dict.Add("DATA_TYPE", DATA_TYPE);
                        dict.Add("IS_NULLABLE", IS_NULLABLE);
                        dict.Add("COLUMN_DEFAULT", COLUMN_DEFAULT);
                        dict.Add("ORDINAL_POSITION", ORDINAL_POSITION);
                        dict.Add("CHARACTER_MAXIMUM_LENGTH", CHARACTER_MAXIMUM_LENGTH);
                        list.Add(dict);
                    }
                }
            }
            return list;
        }

        public static List<Dictionary<string, object>> GetDatabaseInfomation(Profile profile, SQLiteDataReader dataReader, SQLiteConnection connection)
        {
            var list = new List<Dictionary<string, object>>();
            if (profile != null && !string.IsNullOrEmpty(profile.Engine) && profile.Value != null)
            {
                if (profile.Engine.Trim().ToUpper().Equals("SQLITE"))
                {
                    var tableList = new List<object>();
                    while (dataReader.Read())
                        tableList.Add(dataReader["name"]);

                    foreach (var tableName in tableList)
                    {
                        var cmd = new SQLiteCommand("PRAGMA table_info('" + tableName + "')", connection) { CommandTimeout = 0 };
                        var _dataReader = cmd.ExecuteReader();
                        while (_dataReader.Read())
                        {
                            //var cid = _dataReader["cid"];
                            //var pk = _dataReader["pk"];
                            var database = connection.DataSource;
                            var table = tableName;
                            var column = _dataReader["name"];
                            var DATA_TYPE = _dataReader["type"];
                            var IS_NULLABLE = _dataReader["notnull"].ToString() == "0" ? "NO" : "YES";
                            var COLUMN_DEFAULT = _dataReader["dflt_value"];

                            var dict = new Dictionary<string, object>();
                            dict.Add("DATABASE", database);
                            dict.Add("TABLE", table);
                            dict.Add("COLUMN", column);
                            dict.Add("DATA_TYPE", DATA_TYPE);
                            dict.Add("IS_NULLABLE", IS_NULLABLE);
                            dict.Add("COLUMN_DEFAULT", COLUMN_DEFAULT);
                            dict.Add("ORDINAL_POSITION", "-1");
                            dict.Add("CHARACTER_MAXIMUM_LENGTH", "-1");
                            list.Add(dict);
                        }
                        _dataReader.Close();
                    }
                }
            }
            return list;
        }
        #endregion
    }
}
