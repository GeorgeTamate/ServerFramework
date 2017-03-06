using System;
using System.Text;

namespace PHttp
{
    public class HttpServerUtility
    {
        internal HttpServerUtility()
        {
        }

        public string MachineName
        {
            get { return Environment.MachineName; }
        }

        public string HtmlEncode(string value)
        {
            return HttpUtil.HtmlEncode(value);
        }

        public string HtmlDecode(string value)
        {
            return HttpUtil.HtmlDecode(value);
        }

        public string UrlEncode(string text)
        {
            return Uri.EscapeDataString(text);
        }

        public string UrlDecode(string text)
        {
            return UrlDecode(text, Encoding.UTF8);
        }

        public string UrlDecode(string text, Encoding encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            return HttpUtil.UriDecode(text, encoding);
        }

        // Generated while finding out how HttpUtil will be defined.
        private class HttpUtil
        {
            internal static string HtmlDecode(string value)
            {
                throw new NotImplementedException();
            }

            internal static string HtmlEncode(string value)
            {
                throw new NotImplementedException();
            }

            internal static string UriDecode(string text, Encoding encoding)
            {
                throw new NotImplementedException();
            }
        }
    }
}