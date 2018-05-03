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

using blis.Core.Configurations;
using blis.Core.Extensions;
using blis.Core.Extensions.V8.Helpers;
using blis.Core.Handlers;
using blis.Core.Helpers;
using LitJson;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xilium.CefGlue;

namespace blis.Windows.Extensions.V8.CefGlue
{
    public class DatabaseObject : CefV8Handler
    {
        #region Variables
        private CefV8Value Handler;
        private Configuration configuration;
        #endregion

        #region Constructor
        public DatabaseObject(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            configuration = Extension.GetConfiguration();
        }
        #endregion

        #region CreateObject
        public CefV8Value CreateObject()
        {
            Handler = CefV8Value.CreateObject();
            Handler.SetValue("query", CefV8Value.CreateFunction("query", this), CefV8PropertyAttribute.None);
            Handler.SetValue("reader", CefV8Value.CreateFunction("reader", this), CefV8PropertyAttribute.None);
            return Handler;
        }
        #endregion

        #region Execute
        protected override bool Execute(string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
        {
            returnValue = CefV8Value.CreateNull();
            exception = null;

            try
            {
                var context = CefV8Context.GetCurrentContext();
                var browser = context.GetBrowser();
                var frame = browser.GetMainFrame();
                
                if (name == "query")
                {
                    if (arguments.Length >= 4)
                    {
                        var sql = arguments[0].GetStringValue();
                        var connection = arguments[1].GetStringValue();
                        var returntype = arguments[2].GetStringValue();
                        var executionMode = arguments[3].GetStringValue();
                        var callback = (arguments.Length == 5 ? arguments[4] : null);

                        if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(connection) && !string.IsNullOrEmpty(returntype) && !string.IsNullOrEmpty(executionMode))
                        {
                            if (callback != null && callback.IsFunction)
                            {
                                var runner = CefTaskRunner.GetForCurrentThread();
                                new Task(() =>
                                {
                                    context.Enter();
                                    runner.PostTask(new CallbackTask(context, callback, query(sql, connection, returntype, executionMode)));
                                    context.Exit();
                                }).Start();
                            }
                            else returnValue = CefV8Value.CreateString(query(sql, connection, returntype, executionMode));
                        }
                        else returnValue = Extension.DontMeetRequirements(context, callback);
                    }
                    else returnValue = Extension.DontMeetRequirements(context, null);
                }
                else if (name == "reader")
                {
                    if (arguments.Length >= 4)
                    {
                        var sql         = arguments[0].GetStringValue();
                        var connection  = arguments[1].GetStringValue();
                        var returntype  = arguments[2].GetStringValue();
                        var callback    = arguments[3];

                        if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(connection) && !string.IsNullOrEmpty(returntype) && callback != null && callback.IsFunction)
                        {
                            var runner = CefTaskRunner.GetForCurrentThread();
                            new Task(() =>
                            {
                                context.Enter();
                                try
                                {
                                    var db = new ConnectionHelper(connection);
                                    if (db != null)
                                    {
                                        db.Open();
                                        var cmd = db.CreateCommand(sql);
                                        if (cmd != null)
                                        {
                                            var dataReader = cmd.ExecuteReader();
                                            if (dataReader != null)
                                            {
                                                var columns = Enumerable.Range(0, dataReader.FieldCount()).Select(dataReader.GetName).ToList();
                                                if (returntype.ToLower().Equals("json"))
                                                {
                                                    while (dataReader.Read())
                                                    {
                                                        var row = new Dictionary<string, object>();
                                                        for (int x = 0; x < columns.Count; x++)
                                                        {
                                                            var column = columns[x];
                                                            row.Add(columns[x], Extension.ObjectToString(dataReader.GetValue(dataReader.GetOrdinal(column))));
                                                        }
                                                        runner.PostTask(new CallbackTask(context, callback, JsonMapper.ToJson(row), true, false));
                                                    }
                                                }
                                                else if (returntype.ToLower().Equals("xml"))
                                                {
                                                    while (dataReader.Read())
                                                    {
                                                        var row = new Dictionary<string, object>();
                                                        for (int x = 0; x < columns.Count; x++)
                                                        {
                                                            var column = columns[x];
                                                            row.Add(columns[x], Extension.ObjectToString(dataReader.GetValue(dataReader.GetOrdinal(column))));
                                                        }
                                                        var doc = new XElement("Item", row.Select(x => new XElement(x.Key, x.Value)));
                                                        runner.PostTask(new CallbackTask(context, callback, doc.ToString(), true, false));
                                                    }
                                                }
                                                if (returntype.ToLower().Equals("json"))
                                                {
                                                    var row = new Dictionary<string, object>();
                                                    runner.PostTask(new CallbackTask(context, callback, JsonMapper.ToJson(row), false, false));
                                                }
                                                else if (returntype.ToLower().Equals("xml"))
                                                {
                                                    var doc = new XElement("Item");
                                                    runner.PostTask(new CallbackTask(context, callback, doc.ToString(), false, false));
                                                }
                                                dataReader.Close();
                                            }
                                        }
                                        db.Close();
                                    }
                                    else
                                    {
                                        var msg = new StatusHelper("Error", "Failed to create database connection");
                                        runner.PostTask(new CallbackTask(context, callback, JsonMapper.ToJson(msg), false, true));
                                    }
                                }   
                                catch(Exception ex)
                                {
                                    var msg = new StatusHelper("Error", ex.Message);
                                    runner.PostTask(new CallbackTask(context, callback, JsonMapper.ToJson(msg), false, true));
                                }
                                context.Exit();
                            }).Start();
                        }
                        else returnValue = Extension.DontMeetRequirements(context, callback);
                    }
                    else returnValue = Extension.DontMeetRequirements(context, null);
                }

                return true;
            }
            catch (Exception ex)
            {
                returnValue = CefV8Value.CreateNull();
                exception = ex.Message;
                return true;
            }
        }
        #endregion
        
        #region query
        public string query(string sql, string connection, string returntype, string executionMode)
        {
            var results = "[]";
            if (configuration != null && configuration.Profiles != null)
            {
                if (!string.IsNullOrEmpty(connection))
                {
                    var profile = configuration.Profiles.FirstOrDefault(ss => !string.IsNullOrEmpty(ss.Key) && ss.Key.ToLower().Equals(connection.ToLower()));
                    if (profile != null && profile.Value != null && !string.IsNullOrEmpty(profile.Engine))
                    {
                        if (!string.IsNullOrEmpty(sql))
                        {
                            var connectionString = profile.Value.ToString();
                            var engine = profile.Engine;
                            if (executionMode == null) executionMode = "reader";
                            if (engine.ToLower().Equals("mysql")) results = MySQL.Parse(sql, connectionString, returntype, executionMode);
                            else if (engine.ToLower().Equals("sql")) results = SQL.Parse(sql, connectionString, returntype, executionMode);
                            else if (engine.ToLower().Equals("sqlite"))
                            {
                                var _connectionString = "";
                                var parts = connectionString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
                                foreach (var part in parts)
                                {
                                    var sections = part.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                                    if (sections.Count == 2)
                                    {
                                        var key = sections[0];
                                        var value = sections[1];
                                        if (key.ToLower() == "data source")
                                        {
                                            string projectRootPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
                                            string projectPath = Path.Combine(projectRootPath, "Web", configuration.Application);
                                            var dbPath = Path.Combine(projectPath, value);
                                            var dir = Path.GetDirectoryName(dbPath);
                                            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                                            if (!File.Exists(dbPath)) SQLiteConnection.CreateFile(dbPath);
                                            _connectionString += key + "=" + dbPath + ";";
                                        }
                                        else _connectionString += key + "=" + value + ";";
                                    }
                                }
                                results = SQLite.Parse(sql, _connectionString, returntype, executionMode);
                            }
                            else throw new Exception("Connection profile engine cannot be found");
                        }
                        else throw new Exception("SQL query cannot be empty");
                    }
                    else throw new Exception("Connection profile cannot be found");
                }
                else throw new Exception("Connection profile cannot be empty");
            }
            return results;
        }
        #endregion
    }
}
