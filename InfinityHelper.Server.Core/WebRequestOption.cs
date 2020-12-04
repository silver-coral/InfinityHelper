using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    /// <summary>
    /// 定义一组自定义Web请求的参数
    /// </summary>
    public class WebRequestOptions
    {
        private string _url;

        private WebRequestOptions()
        {
            this.CurrentCookies = new List<Cookie>(); 
            this.Headers = new Dictionary<string, string>();
            this.UpdateCookieAfterRequest = true;
        }

        public WebRequestOptions(string url)
            : this()
        {
            this._url = url;
        }
       

        public string RequestUrl { get { return _url; } set { _url = value; } }
        public List<Cookie> CurrentCookies { get; set; }
        public Dictionary<string, string> Headers { get; set; }       
        public bool UpdateCookieAfterRequest { get; set; }
        public string CookieSourceUrl { get; set; }
        public string UserAgent { get; set; }
        public string ContentType { get; set; }
        public string Referer { get; set; }
        public System.Text.Encoding ContentEncoding { get; set; }
        public int Timeout { get; set; }

        public static string DefaultUserAgent { get { return "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36"; } }
        public static int DefaultTimeout { get { return 5000; } }
        public static System.Text.Encoding DefaultContentEncoding { get { return System.Text.Encoding.Default; } }
    }
}
