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
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace blis.Linux.Extensions.V8.Helpers
{
    public class CommandHelper
    {
        private dynamic command;

        public CommandHelper()
        {
        }

        public CommandHelper(SqlCommand command)
        {
            this.command = command;
        }

        public CommandHelper(MySqlCommand command)
        {
            this.command = command;
        }

        public CommandHelper(SQLiteCommand command)
        {
            this.command = command;
        }

        public DataReaderHelper ExecuteReader()
        {
            try
            {
                if (this.command != null && command.Connection != null && command.Connection.State == ConnectionState.Closed) command.Connection.Open();
                if (this.command != null && command.Connection != null && command.Connection.State == ConnectionState.Open) return new DataReaderHelper(command.ExecuteReader());
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }

            return null;
        }

        public long LastInsertedId()
        {
            try
            {
                if (this.command != null && command.Connection != null && command.Connection.State == ConnectionState.Closed) command.Connection.Open();
                if (this.command != null && command.Connection != null && command.Connection.State == ConnectionState.Open) return command.LastInsertedId;
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }

            return -1;
        }

        public int ExecuteNonQuery()
        {
            try
            {
                if (this.command != null && command.Connection != null && command.Connection.State == ConnectionState.Closed) command.Connection.Open();
                if (this.command != null && command.Connection != null && command.Connection.State == ConnectionState.Open) return command.ExecuteNonQuery();
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }

            return -1;
        }

        public object ExecuteScalar()
        {
            try
            {
                if (this.command != null && command.Connection != null && command.Connection.State == ConnectionState.Closed) command.Connection.Open();
                if (this.command != null && command.Connection != null && command.Connection.State == ConnectionState.Open) return command.ExecuteScalar();
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }

            return null;
        }

        public void AddParameter(string parameter, string type, object value)
        {
            try
            {
                if (this.command  != null && this.command is SqlCommand)
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
                else if (this.command != null && this.command is MySqlCommand)
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
                else if (this.command != null && this.command is SQLiteCommand)
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
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
        }
    }
}
