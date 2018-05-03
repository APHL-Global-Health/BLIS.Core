using blis.Core.Configurations;
using blis.Core.Extensions;
using blis.Core.Infrastructure;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using Xilium.CefGlue;

namespace blis.Helpers
{
	public class PostHelper
	{
		public NameValueCollection Data
        {
            get
            {
                try { return HttpUtility.ParseQueryString(Raw); }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
                return null;
            }
        }
        public string Raw { get; private set; }
        private Configuration configuration;

        public PostHelper()
		{
            Raw = "";
            configuration = Extension.GetConfiguration();
        }

        public PostHelper(CefPostDataElement element)
            :this()
        {
            var numBytes = (int)element.BytesCount;
            if (numBytes > 0)
            {
                var bytes = element.GetBytes();
                Raw = Encoding.Default.GetString(bytes);
            }
        }

        public PostHelper(object param)
            : this()
        {
			if(param != null)
			{
				try { Raw = param.ToString(); }
				catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); } 
			}
		}

        public void Append(CefPostDataElement element)
        {
            var numBytes = (int)element.BytesCount;
            if (numBytes > 0)
            {
                var bytes = element.GetBytes();
                Raw += Encoding.Default.GetString(bytes);
            }
        }

        public string Get(string key)
		{
			if (Data != null) return Data.Get(key); // DecodeUrlString(Data.Get(key));
			else return null;
		}

		public static string DecodeUrlString(string url, bool recursive = false)
		{
			if(recursive)
			{
				string newUrl;
				while ((newUrl = Uri.UnescapeDataString(url)) != url)
					url = newUrl;
				return newUrl;
			}
			else return Uri.UnescapeDataString(url);
		}
	}
}

