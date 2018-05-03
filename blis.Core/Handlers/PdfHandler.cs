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
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xilium.CefGlue;
using PdfSharp.Pdf;
using blis.Core.Helpers;
using System.Reflection;
using blis.Core.Configurations;
using blis.Core.Infrastructure;
using LitJson;
using blis.Core.PDF;
#endregion

namespace blis.Core.Handlers
{
    public class PdfHandler : CefResourceHandler
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

        private object pdf;
        private int pages;
        private Configuration configuration;
        #endregion

        #region Constructor
        public PdfHandler(string pdfString)
        {
            #region Initialize
            this.pdf = pdfString;

            MimeType = MimeMapper.GetMimeType("pdf");
            StatusCode = 200;
            StatusText = "OK";
            Headers = new NameValueCollection();
            //Stream = new MemoryStream(Data);
            #endregion

            var path = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            if (File.Exists(Path.Combine(path, "Configurations.xml")))
                configuration = Configuration.Load(Path.Combine(path, "Configurations.xml"));
        }

        public PdfHandler(PdfDocument pdf, int pages)
        {
            #region Initialize
            this.pdf = pdf;
            this.pages = pages;

            MimeType = MimeMapper.GetMimeType("pdf");
            StatusCode = 200;
            StatusText = "OK";
            Headers = new NameValueCollection();
            #endregion
        }
        #endregion

        #region ProcessRequest        
        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            Task task = new Task(() =>
            {
                try
                {
                    var Title = "";
                    byte[] pdfBytes = null;
                    if (this.pdf is string)
                    {
                        //var serializer = new JavaScriptSerializer();
                        //serializer.MaxJsonLength = Int32.MaxValue;
                        //serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

                        var pdf = new iPDF(configuration.Application, Environment.CurrentDirectory, JsonMapper.ToObject<object>(this.pdf.ToString()));
                        pdfBytes = Data = iPDF.CreatePDF(pdf);

                        Title = pdf.Properties.Title;
                        pages = pdf.Properties.Items;
                    }
                    else if(this.pdf is PdfDocument)
                    {
                        var doc = ((PdfDocument)this.pdf);
                        Title = doc.Info.Title;
                    }

                    var dir = Environment.CurrentDirectory + "\\Web\\" + configuration.Application + "\\reports";
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                    var dt = DateTime.Now.ToString("yyyyMMddHmmssfff");
                    var file = Title.Replace(" ", "_") + "_" + dt + "_" + pages + ".pdf";
                    var path = dir + "\\" + file;
                    if (this.pdf is string && pdfBytes != null) File.WriteAllBytes(path, pdfBytes);
                    else if (this.pdf is PdfDocument) ((PdfDocument)this.pdf).Save(path);

                    var echo = JsonMapper.ToJson(new FileHelper("OK", "reports/" + file, Title, pages));

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
