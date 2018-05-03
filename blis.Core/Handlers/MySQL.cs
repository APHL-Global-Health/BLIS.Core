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
using System.IO;
using blis.Helpers;
using System.Collections.Specialized;
using System;
using MySql.Data.MySqlClient;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xilium.CefGlue;
using blis.Core.Extensions;
using blis.Core.Helpers;
using blis.Core.Infrastructure;
using LitJson;
#endregion

namespace blis.Core.Handlers
{
    public class MySQL : CefResourceHandler
    {
        #region Variables
        private byte[] Data;

        private int pos;

        /// <summary>
        /// Gets or sets the http status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the status text.
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// Gets or sets the Mime Type.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        /// <value>The headers.</value>
        public NameValueCollection Headers { get; private set; }

        private string sql;
        private string connectionString;
        private string returnType;
        private string executionMode;
        #endregion

        #region Constructor
        public MySQL(string sql, string connectionString, string returnType, string executionMode)
        {
            #region Initialize           
            MimeType = MimeMapper.GetMimeType(returnType);
            if (string.IsNullOrEmpty(MimeType))
            {
                returnType = "json";
                MimeType = MimeMapper.GetMimeType(returnType);
            }

            this.sql = sql;
            this.connectionString = connectionString;
            this.returnType = returnType;
            this.executionMode = executionMode;
            //Data = Encoding.UTF8.GetBytes(Parse(sql, connectionString, returnType, executionMode));

            StatusCode = 200;
            StatusText = "OK";
            Headers = new NameValueCollection();
            //Stream = new MemoryStream(Data);
            #endregion

        }
        #endregion

        #region ProcessRequest
        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            //Uri uri = new Uri(request.Url);
            //Debug.WriteLine("ProcessRequestAsync: " + request.Url);
            Task task = new Task(() =>
            {
                Data = Encoding.UTF8.GetBytes(Parse(sql, connectionString, returnType, executionMode));
                callback.Continue();
            });
            task.Start();
            return true;
        }
        #endregion

        #region GetResponseHeaders
        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            response.MimeType = MimeType;
            response.Status = StatusCode;
            response.StatusText = StatusText;
            if (Headers.Count > 0) response.SetHeaderMap(Headers);
            responseLength = (Data == null ? 0 : Data.LongLength);
            redirectUrl = null;
        }
        #endregion

        #region ReadResponse
        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            if (Data == null || (bytesToRead == 0 || (Data != null && pos >= Data.Length)))
            {
                bytesRead = 0;
                return false;
            }
            else
            {
                response.Write(Data, pos, bytesToRead);
                pos += bytesToRead;
                bytesRead = bytesToRead;
                return true;
            }
        }
        #endregion

        #region CanGetCookie
        protected override bool CanGetCookie(CefCookie cookie)
        {
            return false;
        }
        #endregion

        #region CanSetCookie
        protected override bool CanSetCookie(CefCookie cookie)
        {
            return false;
        }
        #endregion

        #region Cancel
        protected override void Cancel()
        {
        }
        #endregion

        #region ToReturnType
        public static string ToReturnType(MySqlDataReader dataReader, string returnType)
        {
            var columns = Enumerable.Range(0, dataReader.FieldCount).Select(dataReader.GetName).ToList();
            if (returnType.ToLower().Equals("json"))
            {
                var rows = new List<Dictionary<string, object>>();
                while (dataReader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int x = 0; x < columns.Count; x++)
                    {
                        var column = columns[x];
                        row.Add(columns[x], Extension.ObjectToString(dataReader[column]));
                    }
                    rows.Add(row);
                }

                return JsonMapper.ToJson(rows);
            }
            else if (returnType.ToLower().Equals("xml"))
            {
                var doc = new XElement("Data");
                while (dataReader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int x = 0; x < columns.Count; x++)
                    {
                        var column = columns[x];
                        row.Add(columns[x], Extension.ObjectToString(dataReader[column]));
                    }
                    doc.Add(new XElement("Item", row.Select(x => new XElement(x.Key, x.Value))));
                }
                return doc.ToString();
            }

            return "";
        }
        #endregion

        #region SafelyEscapeInvalidXMLCharacters
        public static string SafelyEscapeInvalidXMLCharacters(object item)
        {
            if (item == null) return "";
            else
            {
                var xmlItem = item.ToString();
                xmlItem = xmlItem.Replace("&", "&amp;");
                xmlItem = xmlItem.Replace("<", "&lt;");
                xmlItem = xmlItem.Replace(">", "&gt;");
                xmlItem = xmlItem.Replace("\"", "&quot;");
                xmlItem = xmlItem.Replace("'", "&apos;");

                return xmlItem;
            }
        }
        #endregion

        #region Parse
        public static string Parse(string sql, string connectionString, string returnType, string executionMode)
        {
            string data = "";
            try
            {
				var connection = new MySqlConnection(connectionString);
				connection.Open();
				var cmd = new MySqlCommand(sql, connection) { CommandTimeout = 0 };
                if (executionMode == "reader")
                {
                    var dataReader = cmd.ExecuteReader();
                    data = ToReturnType(dataReader, returnType);
                    dataReader.Close();
                }
                else if (executionMode == "writer")
                {
                    cmd.ExecuteNonQuery();
                    var status = new StatusHelper("OK", "");
                    data = status.ToReturnType(returnType, typeof(StatusHelper));
                }
				connection.Close();
            }
            catch (Exception ex)
            {
				Log.Trace(ex.Message, ex.StackTrace);
                var error = new ErrorHelper(ex.Message, ex.StackTrace);
                data = error.ToReturnType(returnType, typeof(ErrorHelper));
            }

            return data;
        }
        #endregion
    }
}
