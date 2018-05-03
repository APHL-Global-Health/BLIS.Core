#region Using
using blis.Core.Configurations;
using blis.Core.Helpers;
using blis.Core.Infrastructure;
using blis.Core.Zip;
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
    public class ZipHandler : CefResourceHandler
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

        private string zipString;
        private Configuration configuration;
        #endregion

        #region Constructor
        public ZipHandler(string zipString)
        {
            #region Initialize
            this.zipString = zipString;

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
                    var zip = new iZip(configuration.Application, Environment.CurrentDirectory, JsonMapper.ToObject<object>(zipString));
                    var zipBytes = Data = iZip.CreateZip(zip);

                    var dir = Environment.CurrentDirectory + "\\Web\\" + configuration.Application + "\\reports";
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                    var dt = DateTime.Now.ToString("yyyyMMddHmmssfff");
                    var file = zip.Properties.Title.Replace(" ", "_") + "_" + dt + "_" + zip.Properties.Items + ".zip";
                    var path = dir + "\\" + file;
                    File.WriteAllBytes(path, zipBytes);

                    var echo = JsonMapper.ToJson(new FileHelper("OK", "reports/" + file, zip.Properties.Title, zip.Properties.Items));

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
