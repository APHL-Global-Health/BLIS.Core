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
using System.Collections.Specialized;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xilium.CefGlue;
using blis.Core.Configurations;
using System.Reflection;
using LitJson;
using blis.Core.Infrastructure;
using blis.Core.Helpers;
#endregion

namespace blis.Core.Handlers
{
    public class DatabaseHandler : CefResourceHandler
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

        private string profileName;
        private string query;
        private Configuration configuration;
        #endregion

        #region Constructor
        public DatabaseHandler(string profileName, string query)
        {
            #region Initialize
            this.profileName = profileName;
            this.query = query;

            MimeType = MimeMapper.GetMimeType("zip");
            StatusCode = 200;
            StatusText = "OK";
            Headers = new NameValueCollection();
            #endregion

            var path = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            if (File.Exists(Path.Combine(path, "Configurations.xml")))
                configuration = Configuration.Load(Path.Combine(path, "Configurations.xml"));
        }
        #endregion
        
        #region ProcessRequest
        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            Task task = new Task(() =>
            {
                try
                {
                    if (configuration != null && configuration.Profiles != null && !string.IsNullOrEmpty(this.profileName))
                    {
                        Profile profile = configuration.Profiles.FirstOrDefault(ss => !string.IsNullOrEmpty(ss.Key) && ss.Key.ToLower().Equals(this.profileName.ToLower()));
                        if (profile != null && !string.IsNullOrEmpty(profile.Engine) && profile.Value != null)
                        {
                            var echo = JsonMapper.ToJson(new StatusHelper("Check databse profile"));

                            var dir = Environment.CurrentDirectory + "\\Web\\" + configuration.Application + "\\reports";
                            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                            var dt = DateTime.Now.ToString("yyyyMMddHmmssfff");
                            var file = this.profileName.Replace(" ", "_") + "_" + dt + ".zip";
                            var path = dir + "\\" + file;

                            var SQLpath = @"db.sql";
                            var dbValue = "database";
                            if (profile.Engine.Trim().ToUpper().Equals("SQL") || profile.Engine.Trim().ToUpper().Equals("MYSQL")) dbValue = "database";
                            else if (profile.Engine.Trim().ToUpper().Equals("SQLITE")) dbValue = "data source";

                            var parts = profile.Value.ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
                            foreach (var part in parts)
                            {
                                var sections = part.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                                if (sections.Count == 2)
                                {
                                    var key = sections[0];
                                    var value = sections[1];
                                    if (key.ToLower() == dbValue)
                                    {
                                        SQLpath = SQLpath + @".sql";
                                        break;
                                    }
                                }
                            }

                            if (profile.Engine.Trim().ToUpper().Equals("SQL"))
                            {
                                var connectionString = profile.Value.ToString();
                            }
                            else if (profile.Engine.Trim().ToUpper().Equals("MYSQL"))
                            {
                                var connectionString = profile.Value.ToString();

                                if (this.query.ToLower() == "database/backup")
                                {
                                    /*using (MemoryStream ms = new MemoryStream())
                                    {
                                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                                        {
                                            using (MySqlCommand cmd = new MySqlCommand())
                                            {
                                                using (MySqlBackup mb = new MySqlBackup(cmd))
                                                {
                                                    cmd.Connection = conn;
                                                    conn.Open();
                                                    //mb.ExportInfo.AddDropDatabase = true;
                                                    mb.ExportInfo.AddCreateDatabase = true;
                                                    mb.ExportInfo.ExportTableStructure = true;
                                                    mb.ExportInfo.ExportRows = true;
                                                    mb.ExportToMemoryStream(ms);

                                                    using (ZipFile zip = new ZipFile())
                                                    {
                                                        zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                                                        //zip.Password = zipPassword;
                                                        zip.Encryption = EncryptionAlgorithm.PkzipWeak; // the default: you might need to select the proper value here
                                                        zip.StatusMessageTextWriter = Console.Out;
                                                        zip.AddEntry(SQLpath, ms.ToArray());
                                                        zip.Save(path);

                                                        echo = serializer.Serialize(new FileHelper("OK", "reports/" + file, file, 1));
                                                    }
                                                }
                                            }
                                        }
                                    }*/
                                }
                                else if (this.query.ToLower() == "database/restore")
                                {
                                    //var path = Environment.CurrentDirectory + @"\Reports\";
                                    //var SQLpath = path + @"kagacotz_messaging_db - " + DateTime.Now.ToString("ddMMMyyyy H.mm.ss.fff") + ".sql";

                                    //using (var zip = ZipFile.Read(file))
                                    //{
                                    //    //zip.Password = zipPassword;
                                    //    zip.Encryption = EncryptionAlgorithm.PkzipWeak;
                                    //    zip.StatusMessageTextWriter = Console.Out;
                                    //    var entry = zip[SQLpath];
                                    //    if (entry != null)
                                    //    {
                                    //        using (var stream = new MemoryStream())
                                    //        {
                                    //            entry.Extract(stream);
                                    //            stream.Seek(0, SeekOrigin.Begin);
                                    //
                                    //            using (MySqlConnection conn = new MySqlConnection(connectionString))
                                    //            {
                                    //                using (MySqlCommand cmd = new MySqlCommand())
                                    //                {
                                    //                    using (MySqlBackup mb = new MySqlBackup(cmd))
                                    //                    {
                                    //                        cmd.Connection = conn;
                                    //                        conn.Open();
                                    //                        mb.ImportInfo.TargetDatabase = Path.GetFileNameWithoutExtension(SQLpath);
                                    //                        mb.ImportInfo.DatabaseDefaultCharSet = "utf8";
                                    //                        mb.ImportInfo.DropDatabase = true;
                                    //                        mb.ImportFromMemoryStream(stream);
                                    //                    }
                                    //                }
                                    //            }
                                    //            echo = serializer.Serialize(new StatusHelper("OK", SQLpath));
                                    //            //System.IO.File.Delete(SQLpath);
                                    //        }
                                    //    }
                                    //}
                                }
                            }
                            else if (profile.Engine.Trim().ToUpper().Equals("SQLITE"))
                            {
                                var connectionString = profile.Value.ToString();
                            }

                            Data = Encoding.UTF8.GetBytes(echo);
                        }
                        else
                        {
                            var echo = JsonMapper.ToJson(new ErrorHelper("Check database profile", ""));

                            Data = Encoding.UTF8.GetBytes(echo);
                        }
                    }
                    else
                    {
                        var echo = JsonMapper.ToJson(new ErrorHelper("Databse Profile does not exist", ""));

                        Data = Encoding.UTF8.GetBytes(echo);
                    }
                }
                catch (Exception ex)
                {
                    Log.Trace(ex.Message, ex.StackTrace);
                    var echo = JsonMapper.ToJson(new ErrorHelper(ex.Message, ex.StackTrace));

                    Data = Encoding.UTF8.GetBytes(echo);
                }

                callback.Continue();
            });
            task.Start();

            callback.Continue();
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
    }
}
