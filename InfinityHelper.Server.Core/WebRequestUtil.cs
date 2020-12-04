using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class WebRequestUtil
    {
        private static string AppendQuery(string url,  Dictionary<string,string> query)
        {
            if (query.Count > 0)
            {
                string queryString = string.Join("&", query.Select(p => string.Format("{0}={1}", p.Key, p.Value)));
                if (url.Contains('?'))
                {
                    url = string.Format("{0}&{1}", url, queryString);
                }
                else
                {
                    url = string.Format("{0}?{1}", url, queryString);
                }
            }
            return url;
        }

        private static void AppendPostData(HttpWebRequest request, object postData, string type = "json")
        {
            if(request.Method == "POST" && postData != null)
            {
                string postStr = string.Empty;
                if (type == "json") //{ "p1":"aaa", "p2":"bbb", "p3":"ccc" }
                {
                    request.ContentType = "application/json;charset=UTF-8";
                    postStr = JsonUtil.Serialize(postData, true);
                }
                else if(type == "form") //p1=aaa&p2=bbb&p3=ccc
                {
                    request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                    StringBuilder sb = new StringBuilder(128);
                    foreach (PropertyInfo p in postData.GetType().GetProperties())
                    {
                        object value = p.GetValue(postData);
                        if (value != null)
                        {                           
                            sb.AppendFormat("{0}={1}&", p.Name, Convert.ToString(value));
                        }
                    }
                    if (sb.Length > 0)
                    {
                        postStr = sb.ToString().Substring(0, sb.Length - 1);
                    }
                }

                //写入RequestStream
                if (!string.IsNullOrEmpty(postStr))
                {
                    byte[] byteData = System.Text.Encoding.UTF8.GetBytes(postStr.ToString());
                    request.ContentLength = byteData.LongLength;
                    using (Stream s = request.GetRequestStream())
                    {
                        s.Write(byteData, 0, byteData.Length);
                        s.Flush();
                        s.Close();
                    }
                }
            }            
        }

        /// <summary>
        /// 创建WebRequest
        /// </summary>
        /// <param name="option"></param>
        /// <param name="method"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private static HttpWebRequest CreateRequest(WebRequestOptions option, string method, Dictionary<string, string> query = null)
        {
            System.Net.ServicePointManager.Expect100Continue = false; //源自HTTP1.1协议的一个规范： 100(Continue)
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string url = option.RequestUrl;            
            if(query != null)
            {
                url = AppendQuery(url, query);
            }
            Uri uri = new Uri(url, UriKind.RelativeOrAbsolute);

            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;          
            request.Method = method;
            request.Accept = "*/*";
            request.UserAgent = WebRequestOptions.DefaultUserAgent;
            if (!string.IsNullOrEmpty(option.UserAgent))
            {
                request.UserAgent = option.UserAgent;
            }
            if (!string.IsNullOrEmpty(option.ContentType))
            {
                request.ContentType = option.ContentType;
            }
            request.Headers["Accept-Encoding"] = "gzip, deflate";
            request.Headers["Accept-Language"] = "zh-CN,zh;q=0.9";
            foreach (var p in option.Headers)
            {
                try
                {
                    request.Headers[p.Key] = p.Value;
                }
                catch { }
            }
            if (!string.IsNullOrEmpty(option.Referer))
            {
                request.Referer = option.Referer;
            }

            request.Timeout = WebRequestOptions.DefaultTimeout;
            if (option.Timeout > 0)
            {
                request.Timeout = option.Timeout;
            }

            request.CookieContainer = new CookieContainer();
            foreach (var cookie in option.CurrentCookies)
            {
                request.CookieContainer.Add(uri, cookie);
            }

            Logger.Info(url);

            return request;
        }

        /// <summary>
        /// 更新cookie
        /// </summary>
        /// <param name="option"></param>
        /// <param name="request"></param>
        private static void UpdateCookie(WebRequestOptions option, HttpWebRequest request)
        {
            if (option.UpdateCookieAfterRequest)
            {
                option.CurrentCookies.Clear();
                Uri cookieUri = request.RequestUri;
                if (!string.IsNullOrEmpty(option.CookieSourceUrl))
                {
                    cookieUri = new Uri(option.CookieSourceUrl);
                }
                foreach (Cookie cookie in request.CookieContainer.GetCookies(cookieUri))
                {
                    var current = option.CurrentCookies.FirstOrDefault(p => p.Name == cookie.Name); //移除重名的cookie
                    if (current != null)
                    {
                        option.CurrentCookies.Remove(current);
                    }
                    option.CurrentCookies.Add(cookie);
                }
            }
        }

        /// <summary>
        /// 读取返回结果
        /// </summary>
        /// <param name="option"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private static string ReadResponse(WebRequestOptions option, HttpWebResponse response)
        {
            string result = null;

            System.Text.Encoding encoding = WebRequestOptions.DefaultContentEncoding;
            if (option.ContentEncoding != null)
            {
                encoding = option.ContentEncoding;
            }

            bool isGzip = (response.ContentEncoding == "gzip");
            using (Stream s = response.GetResponseStream())
            {
                if (isGzip)
                {
                    //gzip
                    using (GZipStream zs = new GZipStream(s, CompressionMode.Decompress))
                    {
                        using (StreamReader sr = new System.IO.StreamReader(zs, encoding))
                        {
                            result = sr.ReadToEnd();
                            sr.Close();
                        }
                        zs.Close();
                    }
                }
                else
                {
                    using (StreamReader sr = new StreamReader(s, encoding))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                }

                s.Close();
            }

            return result;
        }

        /// <summary>
        /// 读取返回结果（二进制）
        /// </summary>
        /// <param name="option"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private static byte[] ReadResponseBytes(WebRequestOptions option, HttpWebResponse response)
        {
            MemoryStream result = new MemoryStream();
            int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];
            int index = 0;
            bool isGzip = (response.ContentEncoding == "gzip");
            using (Stream s = response.GetResponseStream())
            {
                if (isGzip)
                {
                    //gzip
                    using (GZipStream zs = new GZipStream(s, CompressionMode.Decompress))
                    {
                        do
                        {
                            index = zs.Read(buffer, 0, buffer.Length);
                            if (index > 0)
                            {
                                if (index < bufferSize)
                                {
                                    result.Write(buffer, 0, index);
                                }
                                else
                                {
                                    result.Write(buffer, 0, bufferSize);
                                }
                            }
                        }
                        while (index > 0);
                        zs.Close();
                    }
                }
                else
                {
                    do
                    {
                        index = s.Read(buffer, 0, buffer.Length);
                        if (index > 0)
                        {
                            if (index < bufferSize)
                            {
                                result.Write(buffer, 0, index);
                            }
                            else
                            {
                                result.Write(buffer, 0, bufferSize);
                            }
                        }
                    }
                    while (index > 0);
                }

                s.Close();
            }
            return result.ToArray();
        }

        /// <summary>
        /// 发起一个POST请求
        /// </summary>
        /// <param name="option"></param>
        public static void PostNonQuery(WebRequestOptions option, object postData, string type = "json")
        {
            HttpWebRequest request = CreateRequest(option, "POST");
            AppendPostData(request, postData, type);

            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                UpdateCookie(option, request);

                response.Close();
            }
            finally
            {
                request.Abort();
            }
        }

        /// <summary>
        /// 发起一个POST请求，并返回结果字符串
        /// </summary>        
        /// <returns></returns>
        public static string PostStringResult(WebRequestOptions option, object postData, string type = "json")
        {
            HttpWebRequest request = CreateRequest(option, "POST");
            AppendPostData(request, postData, type);

            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                UpdateCookie(option, request);

                string result = ReadResponse(option, response);

                response.Close();

                return result;
            }
            finally
            {
                request.Abort();
            }
        }

        /// <summary>
        /// 发起一个POST请求，并返回二进制
        /// </summary>        
        /// <returns></returns>
        public static byte[] PostBytesResultAsync(WebRequestOptions option, object postData, string type = "json")
        {
            HttpWebRequest request = CreateRequest(option, "POST");
            AppendPostData(request, postData, type);

            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                UpdateCookie(option, request);

                byte[] result = ReadResponseBytes(option, response);

                response.Close();

                return result;
            }
            finally
            {
                request.Abort();
            }
        }

        /// <summary>
        /// 发起一个POST请求，并返回json反序列化的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public static T PostResult<T>(WebRequestOptions option, object postData, string type = "json")
            where T : class
        {
            string resultStr = PostStringResult(option, postData, type);

            return JsonUtil.Deserialize<T>(resultStr);
        }

        #region GET

        /// <summary>
        /// 发起一个GET请求
        /// </summary>
        /// <param name="option"></param>
        public static void GetNonQuery(WebRequestOptions option, Dictionary<string,string> query = null)
        {
            HttpWebRequest request = CreateRequest(option, "GET", query);

            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                UpdateCookie(option, request);

                response.Close();
            }
            finally
            {
                request.Abort();
            }
        }

        /// <summary>
        /// 发起一个GET请求，并返回结果字符串
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static string GetStringResult(WebRequestOptions option, Dictionary<string, string> query = null)
        {
            HttpWebRequest request = CreateRequest(option, "GET", query);
            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                UpdateCookie(option, request);

                string result = ReadResponse(option, response);

                response.Close();

                return result;
            }
            finally
            {
                request.Abort();
            }
        }

        /// <summary>
        /// 发起一个GET请求，并返回二进制
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static byte[] GetBytesResult(WebRequestOptions option, Dictionary<string, string> query = null)
        {
            HttpWebRequest request = CreateRequest(option, "GET", query);
            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                UpdateCookie(option, request);

                byte[] result = ReadResponseBytes(option, response);

                response.Close();

                return result;
            }
            finally
            {
                request.Abort();
            }
        }

        /// <summary>
        /// 发起一个GET请求，并返回json反序列化的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public static T GetResult<T>(WebRequestOptions option, Dictionary<string, string> query = null)
            where T : class
        {
            string resultStr = GetStringResult(option, query);
            return JsonUtil.Deserialize<T>(resultStr);
        }

        #endregion
    }
}
