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
using blis.Core.Excel;
using blis.Core.Helpers;
using blis.Core.Infrastructure;
using blis.Helpers;
using LitJson;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xilium.CefGlue;
#endregion


namespace blis.Core.Handlers
{
    public class ExcelHandler : CefResourceHandler
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

        private string excelString;
        private Configuration configuration;
        #endregion

        #region Constructor
        public ExcelHandler(string excelString)
        {
            #region Initialize
            this.excelString = excelString;

            MimeType = MimeMapper.GetMimeType("xlsx");
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
                    //var serializer = new JavaScriptSerializer();
                    //serializer.MaxJsonLength = Int32.MaxValue;
                    //serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                    var excel = new iExcel(configuration.Application, Environment.CurrentDirectory, JsonMapper.ToObject<object>(excelString));
                    var excelBytes = Data = iExcel.CreateExcel(excel);

                    var dir = Environment.CurrentDirectory + "\\Web\\" + configuration.Application + "\\reports";
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                    var dt = DateTime.Now.ToString("yyyyMMddHmmssfff");
                    var file = excel.Properties.Title.Replace(" ", "_") + "_" + dt + "_" + excel.Properties.Items + ".xlsx";
                    var path = dir + "\\" + file;
                    File.WriteAllBytes(path, excelBytes);

                    var echo = JsonMapper.ToJson(new FileHelper("OK", "reports/" + file, excel.Properties.Title, excel.Properties.Items));

                    Data = Encoding.UTF8.GetBytes(echo);
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
