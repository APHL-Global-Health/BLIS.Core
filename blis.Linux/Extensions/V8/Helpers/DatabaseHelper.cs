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

#region Using
using blis.Core.Configurations;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
#endregion

namespace blis.Linux.Extensions.V8.Helpers
{
    public class DatabaseHelper
    {
        #region Variables
        private static Configuration configuration;
        #endregion

        #region SanityCheck
        public static void SanityCheck()
        {
            if (configuration == null)
            {
                var path = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
                if (File.Exists(Path.Combine(path, "Configurations.xml")))
                    configuration = Configuration.Load(Path.Combine(path, "Configurations.xml"));
            }
        }
        #endregion

        #region Create Connection
        public static dynamic CreateConnection(string name)
        {
            SanityCheck();
            if (configuration != null && configuration.Profiles != null && !string.IsNullOrEmpty(name))
                return CreateConnection(configuration.Profiles.FirstOrDefault(ss => !string.IsNullOrEmpty(ss.Key) && ss.Key.ToLower().Equals(name.ToLower())));
            return null;
        }

        public static dynamic CreateConnection(Profile profile)
        {
            if (profile != null && !string.IsNullOrEmpty(profile.Engine) && profile.Value != null)
            {
                if (profile.Engine.Trim().ToUpper().Equals("SQL")) return new SqlConnection(profile.Value.ToString());
                else if (profile.Engine.Trim().ToUpper().Equals("MYSQL")) return new MySqlConnection(profile.Value.ToString());
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
                    if (!string.IsNullOrEmpty(_connectionString)) return new SQLiteConnection(_connectionString);
                }
            }
            return null;
        }
        #endregion

        #region Create Command
        public static dynamic CreateCommand(string name, string sql, MySqlConnection connection)
        {
            SanityCheck();
            if (configuration != null && configuration.Profiles != null && !string.IsNullOrEmpty(name))
                return CreateCommand(configuration.Profiles.FirstOrDefault(ss => !string.IsNullOrEmpty(ss.Key) && ss.Key.ToLower().Equals(name.ToLower())), sql, connection);
            return null;
        }

        public static dynamic CreateCommand(string name, string sql, SqlConnection connection)
        {
            SanityCheck();
            if (configuration != null && configuration.Profiles != null && !string.IsNullOrEmpty(name))
                return CreateCommand(configuration.Profiles.FirstOrDefault(ss => !string.IsNullOrEmpty(ss.Key) && ss.Key.ToLower().Equals(name.ToLower())), sql, connection);
            return null;
        }

        public static dynamic CreateCommand(string name, string sql, SQLiteConnection connection)
        {
            SanityCheck();
            if (configuration != null && configuration.Profiles != null && !string.IsNullOrEmpty(name))
                return CreateCommand(configuration.Profiles.FirstOrDefault(ss => !string.IsNullOrEmpty(ss.Key) && ss.Key.ToLower().Equals(name.ToLower())), sql, connection);
            return null;
        }

        public static dynamic CreateCommand(Profile profile, string sql, MySqlConnection connection)
        {
            if (profile != null && !string.IsNullOrEmpty(profile.Engine) && profile.Value != null && !string.IsNullOrEmpty(sql) && profile.Engine.Trim().ToUpper().Equals("MYSQL"))
            {
                return new MySqlCommand(sql, connection) { CommandTimeout = 0 };
            }
            return null;
        }

        public static dynamic CreateCommand(Profile profile, string sql, SqlConnection connection)
        {
            if (profile != null && !string.IsNullOrEmpty(profile.Engine) && profile.Value != null && !string.IsNullOrEmpty(sql) && profile.Engine.Trim().ToUpper().Equals("SQL"))
            {
                return new SqlCommand(sql, connection) { CommandTimeout = 0 };
            }
            return null;
        }

        public static dynamic CreateCommand(Profile profile, string sql, SQLiteConnection connection)
        {
            if (profile != null && !string.IsNullOrEmpty(profile.Engine) && profile.Value != null && !string.IsNullOrEmpty(sql) && profile.Engine.Trim().ToUpper().Equals("SQLITE"))
            {
                return new SQLiteCommand(sql, connection) { CommandTimeout = 0 };
            }
            return null;
        }
        #endregion

        #region Add Parameter
        public static void AddParameter(dynamic command, string parameter, string type, object value, string commandType)
        {
            if (command != null && !string.IsNullOrEmpty(parameter) && !string.IsNullOrEmpty(type))
            {
                if (commandType.ToLower().Trim() == "sql")
                {
                    if (type.ToLower().Equals("binary")) command.Parameters.Add(parameter, SqlDbType.Binary).Value = value;
                    else if (type.ToLower().Equals("bit")) command.Parameters.Add(parameter, SqlDbType.Bit).Value = value;
                    else if (type.ToLower().Equals("char")) command.Parameters.Add(parameter, SqlDbType.Char).Value = value;
                    else if (type.ToLower().Equals("date")) command.Parameters.Add(parameter, SqlDbType.Date).Value = value;
                    else if (type.ToLower().Equals("datetime")) command.Parameters.Add(parameter, SqlDbType.DateTime).Value = value;
                    else if (type.ToLower().Equals("datetime2")) command.Parameters.Add(parameter, SqlDbType.DateTime2).Value = value;
                    else if (type.ToLower().Equals("datetimeoffset")) command.Parameters.Add(parameter, SqlDbType.DateTimeOffset).Value = value;
                    else if (type.ToLower().Equals("decimal")) command.Parameters.Add(parameter, SqlDbType.Decimal).Value = value;
                    else if (type.ToLower().Equals("float")) command.Parameters.Add(parameter, SqlDbType.Float).Value = value;
                    else if (type.ToLower().Equals("image")) command.Parameters.Add(parameter, SqlDbType.Image).Value = value;
                    else if (type.ToLower().Equals("int")) command.Parameters.Add(parameter, SqlDbType.Int).Value = value;
                    else if (type.ToLower().Equals("money")) command.Parameters.Add(parameter, SqlDbType.Money).Value = value;
                    else if (type.ToLower().Equals("nchar")) command.Parameters.Add(parameter, SqlDbType.NChar).Value = value;
                    else if (type.ToLower().Equals("ntext")) command.Parameters.Add(parameter, SqlDbType.NText).Value = value;
                    else if (type.ToLower().Equals("nvarchar")) command.Parameters.Add(parameter, SqlDbType.NVarChar).Value = value;
                    else if (type.ToLower().Equals("real")) command.Parameters.Add(parameter, SqlDbType.Real).Value = value;
                    else if (type.ToLower().Equals("smalldatetime")) command.Parameters.Add(parameter, SqlDbType.SmallDateTime).Value = value;
                    else if (type.ToLower().Equals("smallint")) command.Parameters.Add(parameter, SqlDbType.SmallInt).Value = value;
                    else if (type.ToLower().Equals("smallmoney")) command.Parameters.Add(parameter, SqlDbType.SmallMoney).Value = value;
                    else if (type.ToLower().Equals("structured")) command.Parameters.Add(parameter, SqlDbType.Structured).Value = value;
                    else if (type.ToLower().Equals("text")) command.Parameters.Add(parameter, SqlDbType.Text).Value = value;
                    else if (type.ToLower().Equals("time")) command.Parameters.Add(parameter, SqlDbType.Time).Value = value;
                    else if (type.ToLower().Equals("timestamp")) command.Parameters.Add(parameter, SqlDbType.Timestamp).Value = value;
                    else if (type.ToLower().Equals("tinyint")) command.Parameters.Add(parameter, SqlDbType.TinyInt).Value = value;
                    else if (type.ToLower().Equals("udt")) command.Parameters.Add(parameter, SqlDbType.Udt).Value = value;
                    else if (type.ToLower().Equals("uniqueidentifier")) command.Parameters.Add(parameter, SqlDbType.UniqueIdentifier).Value = value;
                    else if (type.ToLower().Equals("varbinary")) command.Parameters.Add(parameter, SqlDbType.VarBinary).Value = value;
                    else if (type.ToLower().Equals("varchar")) command.Parameters.Add(parameter, SqlDbType.VarChar).Value = value;
                    else if (type.ToLower().Equals("variant")) command.Parameters.Add(parameter, SqlDbType.Variant).Value = value;
                    else if (type.ToLower().Equals("xml")) command.Parameters.Add(parameter, SqlDbType.Xml).Value = value;
                }
                else if (commandType.ToLower().Trim() == "mysql")
                {
                    if (type.ToLower().Equals("binary")) command.Parameters.Add(parameter, MySqlDbType.Binary).Value = value;
                    else if (type.ToLower().Equals("bit")) command.Parameters.Add(parameter, MySqlDbType.Bit).Value = value;
                    else if (type.ToLower().Equals("blob")) command.Parameters.Add(parameter, MySqlDbType.Blob).Value = value;
                    else if (type.ToLower().Equals("byte")) command.Parameters.Add(parameter, MySqlDbType.Byte).Value = value;
                    else if (type.ToLower().Equals("date")) command.Parameters.Add(parameter, MySqlDbType.Date).Value = value;
                    else if (type.ToLower().Equals("datetime")) command.Parameters.Add(parameter, MySqlDbType.DateTime).Value = value;
                    else if (type.ToLower().Equals("decimal")) command.Parameters.Add(parameter, MySqlDbType.Decimal).Value = value;
                    else if (type.ToLower().Equals("double")) command.Parameters.Add(parameter, MySqlDbType.Double).Value = value;
                    else if (type.ToLower().Equals("enum")) command.Parameters.Add(parameter, MySqlDbType.Enum).Value = value;
                    else if (type.ToLower().Equals("float")) command.Parameters.Add(parameter, MySqlDbType.Float).Value = value;
                    else if (type.ToLower().Equals("geometry")) command.Parameters.Add(parameter, MySqlDbType.Geometry).Value = value;
                    else if (type.ToLower().Equals("guid")) command.Parameters.Add(parameter, MySqlDbType.Guid).Value = value;
                    else if (type.ToLower().Equals("int16")) command.Parameters.Add(parameter, MySqlDbType.Int16).Value = value;
                    else if (type.ToLower().Equals("int24")) command.Parameters.Add(parameter, MySqlDbType.Int24).Value = value;
                    else if (type.ToLower().Equals("int32")) command.Parameters.Add(parameter, MySqlDbType.Int32).Value = value;
                    else if (type.ToLower().Equals("int64")) command.Parameters.Add(parameter, MySqlDbType.Int64).Value = value;
                    else if (type.ToLower().Equals("json")) command.Parameters.Add(parameter, MySqlDbType.JSON).Value = value;
                    else if (type.ToLower().Equals("longblob")) command.Parameters.Add(parameter, MySqlDbType.LongBlob).Value = value;
                    else if (type.ToLower().Equals("longtext")) command.Parameters.Add(parameter, MySqlDbType.LongText).Value = value;
                    else if (type.ToLower().Equals("mediumblob")) command.Parameters.Add(parameter, MySqlDbType.MediumBlob).Value = value;
                    else if (type.ToLower().Equals("mediumtext")) command.Parameters.Add(parameter, MySqlDbType.MediumText).Value = value;
                    else if (type.ToLower().Equals("newdate")) command.Parameters.Add(parameter, MySqlDbType.Newdate).Value = value;
                    else if (type.ToLower().Equals("newdecimal")) command.Parameters.Add(parameter, MySqlDbType.NewDecimal).Value = value;
                    else if (type.ToLower().Equals("set")) command.Parameters.Add(parameter, MySqlDbType.Set).Value = value;
                    else if (type.ToLower().Equals("string")) command.Parameters.Add(parameter, MySqlDbType.String).Value = value;
                    else if (type.ToLower().Equals("text")) command.Parameters.Add(parameter, MySqlDbType.Text).Value = value;
                    else if (type.ToLower().Equals("time")) command.Parameters.Add(parameter, MySqlDbType.Time).Value = value;
                    else if (type.ToLower().Equals("timestamp")) command.Parameters.Add(parameter, MySqlDbType.Timestamp).Value = value;
                    else if (type.ToLower().Equals("tinyblob")) command.Parameters.Add(parameter, MySqlDbType.TinyBlob).Value = value;
                    else if (type.ToLower().Equals("tinytext")) command.Parameters.Add(parameter, MySqlDbType.TinyText).Value = value;
                    else if (type.ToLower().Equals("ubyte")) command.Parameters.Add(parameter, MySqlDbType.UByte).Value = value;
                    else if (type.ToLower().Equals("uint16")) command.Parameters.Add(parameter, MySqlDbType.UInt16).Value = value;
                    else if (type.ToLower().Equals("uint24")) command.Parameters.Add(parameter, MySqlDbType.UInt24).Value = value;
                    else if (type.ToLower().Equals("uint32")) command.Parameters.Add(parameter, MySqlDbType.UInt32).Value = value;
                    else if (type.ToLower().Equals("uint64")) command.Parameters.Add(parameter, MySqlDbType.UInt64).Value = value;
                    else if (type.ToLower().Equals("varbinary")) command.Parameters.Add(parameter, MySqlDbType.VarBinary).Value = value;
                    else if (type.ToLower().Equals("varchar")) command.Parameters.Add(parameter, MySqlDbType.VarChar).Value = value;
                    else if (type.ToLower().Equals("varstring")) command.Parameters.Add(parameter, MySqlDbType.VarString).Value = value;
                    else if (type.ToLower().Equals("year")) command.Parameters.Add(parameter, MySqlDbType.Year).Value = value;
                }
                else if (commandType.ToLower().Trim() == "sqlite")
                {
                    if (type.ToLower().Equals("binary")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Binary) { Value = value });
                    else if (type.ToLower().Equals("bit")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Bit) { Value = value });
                    else if (type.ToLower().Equals("char")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Char) { Value = value });
                    else if (type.ToLower().Equals("date")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Date) { Value = value });
                    else if (type.ToLower().Equals("datetime")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.DateTime) { Value = value });
                    else if (type.ToLower().Equals("datetime2")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.DateTime2) { Value = value });
                    else if (type.ToLower().Equals("datetimeoffset")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.DateTimeOffset) { Value = value });
                    else if (type.ToLower().Equals("decimal")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Decimal) { Value = value });
                    else if (type.ToLower().Equals("float")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Float) { Value = value });
                    else if (type.ToLower().Equals("image")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Image) { Value = value });
                    else if (type.ToLower().Equals("int")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Int) { Value = value });
                    else if (type.ToLower().Equals("money")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Money) { Value = value });
                    else if (type.ToLower().Equals("nchar")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.NChar) { Value = value });
                    else if (type.ToLower().Equals("ntext")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.NText) { Value = value });
                    else if (type.ToLower().Equals("nvarchar")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.NVarChar) { Value = value });
                    else if (type.ToLower().Equals("real")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Real) { Value = value });
                    else if (type.ToLower().Equals("smalldatetime")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.SmallDateTime) { Value = value });
                    else if (type.ToLower().Equals("smallint")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.SmallInt) { Value = value });
                    else if (type.ToLower().Equals("smallmoney")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.SmallMoney) { Value = value });
                    else if (type.ToLower().Equals("structured")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Structured) { Value = value });
                    else if (type.ToLower().Equals("text")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Text) { Value = value });
                    else if (type.ToLower().Equals("time")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Time) { Value = value });
                    else if (type.ToLower().Equals("timestamp")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Timestamp) { Value = value });
                    else if (type.ToLower().Equals("tinyint")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.TinyInt) { Value = value });
                    else if (type.ToLower().Equals("udt")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Udt) { Value = value });
                    else if (type.ToLower().Equals("uniqueidentifier")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.UniqueIdentifier) { Value = value });
                    else if (type.ToLower().Equals("varbinary")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.VarBinary) { Value = value });
                    else if (type.ToLower().Equals("varchar")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.VarChar) { Value = value });
                    else if (type.ToLower().Equals("variant")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Variant) { Value = value });
                    else if (type.ToLower().Equals("xml")) command.Parameters.Add(new SQLiteParameter(parameter, SqlDbType.Xml) { Value = value });
                }
            }
        }

        #region Add Sql Parameter
        public static void AddParameter(SqlCommand command, string parameter, string type, object value)
        {
            AddParameter(command, parameter, type, value, "sql");
        }

        public static void AddParameter(SqlCommand command, string parameter, string type, string value)
        {
            AddParameter(command, parameter, type, value, "sql");
        }

        public static void AddParameter(SqlCommand command, string parameter, string type, int value)
        {
            AddParameter(command, parameter, type, value, "sql");
        }

        public static void AddParameter(SqlCommand command, string parameter, string type, bool value)
        {
            AddParameter(command, parameter, type, value, "sql");
        }

        public static void AddParameter(SqlCommand command, string parameter, string type, char value)
        {
            AddParameter(command, parameter, type, value, "sql");
        }

        public static void AddParameter(SqlCommand command, string parameter, string type, DateTime value)
        {
            AddParameter(command, parameter, type, value, "sql");
        }
        #endregion

        #region Add MySql Parameter
        public static void AddParameter(MySqlCommand command, string parameter, string type, object value)
        {
            AddParameter(command, parameter, type, value, "mysql");
        }

        public static void AddParameter(MySqlCommand command, string parameter, string type, string value)
        {
            AddParameter(command, parameter, type, value, "mysql");
        }

        public static void AddParameter(MySqlCommand command, string parameter, string type, int value)
        {
            AddParameter(command, parameter, type, value, "mysql");
        }

        public static void AddParameter(MySqlCommand command, string parameter, string type, bool value)
        {
            AddParameter(command, parameter, type, value, "mysql");
        }

        public static void AddParameter(MySqlCommand command, string parameter, string type, char value)
        {
            AddParameter(command, parameter, type, value, "mysql");
        }

        public static void AddParameter(MySqlCommand command, string parameter, string type, DateTime value)
        {
            AddParameter(command, parameter, type, value, "mysql");
        }
        #endregion

        #region Add Sqlite Parameter
        public static void AddParameter(SQLiteCommand command, string parameter, string type, object value)
        {
            AddParameter(command, parameter, type, value, "sqlite");
        }

        public static void AddParameter(SQLiteCommand command, string parameter, string type, string value)
        {
            AddParameter(command, parameter, type, value, "sqlite");
        }

        public static void AddParameter(SQLiteCommand command, string parameter, string type, int value)
        {
            AddParameter(command, parameter, type, value, "sqlite");
        }

        public static void AddParameter(SQLiteCommand command, string parameter, string type, bool value)
        {
            AddParameter(command, parameter, type, value, "sqlite");
        }

        public static void AddParameter(SQLiteCommand command, string parameter, string type, char value)
        {
            AddParameter(command, parameter, type, value, "sqlite");
        }

        public static void AddParameter(SQLiteCommand command, string parameter, string type, DateTime value)
        {
            AddParameter(command, parameter, type, value, "sqlite");
        }
        #endregion
        #endregion

        #region ExecuteReader
        public static dynamic Execute(SqlCommand command)
        {
            return command.ExecuteReader();
        }

        public static dynamic Execute(MySqlCommand command)
        {
            return command.ExecuteReader();
        }

        public static dynamic Execute(SQLiteCommand command)
        {
            return command.ExecuteReader();
        }
        #endregion

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(SqlCommand command)
        {
            return command.ExecuteNonQuery();
        }

        public static int ExecuteNonQuery(MySqlCommand command)
        {
            return command.ExecuteNonQuery();
        }

        public static int ExecuteNonQuery(SQLiteCommand command)
        {
            return command.ExecuteNonQuery();
        }
        #endregion

        #region SQLString Database Infomation
        public static string SQLString_DatabaseInfomation(string name)
        {
            SanityCheck();
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
        public static List<Dictionary<string, object>> GetDatabaseInfomation(string name, MySqlDataReader dataReader, MySqlConnection connection)
        {
            SanityCheck();
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
