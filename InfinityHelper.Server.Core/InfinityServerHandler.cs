using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityServerHandler
    {
        private InfinityServerSite _site;
        private InfinityServerMapping _mapping;
        private readonly IOwinContext _context;
        private readonly ProxyUri _uri;

        public ProxyUri Uri { get { return _uri; } }
        public IOwinContext Context { get { return _context; } }
        public IOwinRequest Request { get { return _context.Request; } }
        public IOwinResponse Response { get { return _context.Response; } }

        public InfinityServerHandler(IOwinContext context)
        {
            this._context = context;
            this._uri = new ProxyUri(context.Request);
            this._site = new InfinityServerSite(this._uri, this.Request, this.Response);
            this._mapping = this._site.MainMapping;
        }

        private void ParseCookies(IOwinRequest request)
        {
            var charId = request.Cookies["charaId"];
            if (!string.IsNullOrEmpty(charId))
            {                
                _site.CurrentCharId = charId;                
                _site.Config = CharacterConfigCache.TryGetValue(charId, CharacterConfigCache.LoadConfig);
                _site.Dynamic = CharacterDynamicCache.TryGetValue(charId, CharacterDynamicCache.LoadDynamic);
                _site.FilterConfig = ItemFilterCache.TryGetValue(charId, ItemFilterCache.LoadConfig);

                string ip = GetRealIP();
                if(!_site.Config.IPList.Contains(ip))
                {
                    _site.Config.IPList.Add(ip);
                    CharacterConfigCache.SaveConfig(_site.Config);
                }
            }
        }

        public static bool IsIPAddress(string str)
        {
            if (string.IsNullOrWhiteSpace(str) || str.Length < 7 || str.Length > 15)
            {
                return false;
            }

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}{1}";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str);
        }

        public string GetRealIP()
        {
            string x_forwarded_for = this.Request.Headers["HTTP_X_FORWARDED_FOR"];
            string result = x_forwarded_for;

            //可能有代理   
            if (!string.IsNullOrWhiteSpace(result))
            {
                //没有"." 肯定是非IP格式  
                if (result.IndexOf(".") == -1)
                {
                    result = null;
                }
                else
                {
                    //有","，估计多个代理。取第一个不是内网的IP。  
                    if (result.IndexOf(",") != -1)
                    {
                        result = result.Replace(" ", string.Empty).Replace("\"", string.Empty);
                        string[] temparyip = result.Split(",;".ToCharArray());
                        if (temparyip != null && temparyip.Length > 0)
                        {
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                //找到不是内网的地址  
                                if (IsIPAddress(temparyip[i])
                                    && temparyip[i].Substring(0, 3) != "10."
                                    && temparyip[i].Substring(0, 7) != "192.168"
                                    && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    result = temparyip[i];
                                }
                            }
                        }
                    }
                    //代理即是IP格式  
                    else if (IsIPAddress(result))
                    {

                    }
                    //代理中的内容非IP  
                    else
                    {
                        result = null;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                result = this.Request.Headers["REMOTE_ADDR"];
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                result = this.Request.RemoteIpAddress;
            }

            return result;
        }

        // <summary>
        /// Process Proxy Request
        /// </summary>
        /// <returns></returns>
        public void ProcessRequest()
        {
            //直接取静态资源
            if (_uri.IsStatic())
            {
                string staticFilePath = _site.GetProxyFilePath(_uri);
                if (File.Exists(staticFilePath))
                {
                    byte[] result = File.ReadAllBytes(staticFilePath);
                    Response.Expires = new DateTimeOffset(DateTime.Now.AddDays(1));
                    Response.Write(result);
                    return;
                }
                else
                {
                    Response.StatusCode = 404;
                    return;
                }
            }

            try
            {
                ParseCookies(this.Request);

                if (_uri.IsApi())
                {
                    var api = _site.TryResolveApi(_uri);
                    if (api != null)
                    {
                        api.Execute();
                    }
                }
                else if (Request.Method == "GET")
                {
                    if (string.IsNullOrEmpty(_site.CurrentCharId) && !_uri.IsLogin())
                    {
                        Response.Redirect(_site.LoginPath);
                    }
                    else if (_site.HasTemplate(_uri))//如果有对应的Template
                    {
                        string dynamicResult = _site.ParseHtml(_uri);
                        Response.Write(dynamicResult);
                    }
                    else//没有Template
                    {
                        if (_uri.IsRoot())
                        {
                            Response.Redirect(_site.DefaultPath);
                        }
                        else
                        {
                            Response.StatusCode = 404;
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Response.StatusCode = 500;
                Response.Write(ex.Message);
            }
            finally { }
        }

        public T GetSourceObject<T>(string path, bool isPost = true, bool useCookie = true)
        {
            string content = GetSourceContent(path, isPost, useCookie);
            return JsonUtil.Deserialize<T>(content);
        }

        public string GetSourceContent(string path, bool isPost = true, bool useCookie = true)
        {
            HttpWebRequest request = GetSourceRequest(path, isPost, useCookie) as HttpWebRequest;
            try
            {                
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                return GetSourceContent(response);
            }
            finally
            {
                request.Abort();                
            }
        }  

        /// <summary>
        /// 复制源Request
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="sourceRequest"></param>
        /// <returns></returns>
        public HttpWebRequest GetSourceRequest(string path, bool isPost = true, bool useCookie = true)
        {
            //源Url，Authority换成源服务器，后面的路径和参数不变
            var sourceUrl = _mapping.Authority + path;
            Logger.Info("{0}-{1}", Request.Method, sourceUrl);

            var sourceRequest = System.Net.WebRequest.Create(sourceUrl) as System.Net.HttpWebRequest;

            //复制Headers
            foreach (string headerKey in Request.Headers.Keys)
            {
                if (headerKey == "Cookie")
                {
                    continue;
                }

                try
                {
                    sourceRequest.Headers.Set(headerKey, Request.Headers[headerKey]);
                }
                catch { }
            }

            //复制其它Header
            sourceRequest.Accept = Request.Accept;
            sourceRequest.Method = Request.Method;

            if (isPost)
            {
                sourceRequest.Method = "POST";
            }

            sourceRequest.UserAgent = Request.Headers["User-Agent"];
            sourceRequest.AllowAutoRedirect = true;
            sourceRequest.ContentType = Request.ContentType;
            sourceRequest.Referer = _mapping.Referer; //Referer
            sourceRequest.Host = _mapping.Domain;
            sourceRequest.KeepAlive = true;
            sourceRequest.Timeout = 20000;

            //复制Cookie
            sourceRequest.CookieContainer = new CookieContainer();
            if (useCookie)
            {
                foreach (var p in Request.Cookies)
                {
                    Cookie cookie = new Cookie(p.Key, p.Value, "/", _mapping.Domain);
                    sourceRequest.CookieContainer.Add(cookie);
                }
            }

            //复制POST的内容
            if (sourceRequest.Method.ToUpper() == "POST")
            {
                Request.Body.CopyTo(sourceRequest.GetRequestStream());
            }

            return sourceRequest;
        }

        /// <summary>
        /// 获得源Response的内容
        /// </summary>
        /// <returns></returns>
        public string GetSourceContent(HttpWebResponse sourceResp)
        {
            string result = null;

            //ProxyResponse.ContentType = sourceResp.ContentType;
            //ProxyResponse.StatusCode = Convert.ToInt32(sourceResp.StatusCode);

            //复制cookie
            foreach (System.Net.Cookie ck in sourceResp.Cookies)
            {
                Response.Cookies.Append(ck.Name, ck.Value);
            }

            //从返回的流中获得内容
            using (var respStream = sourceResp.GetResponseStream())
            {
                bool isGzip = (sourceResp.ContentEncoding == "gzip");
                if (isGzip)
                {
                    //gzip
                    using (GZipStream zs = new GZipStream(respStream, CompressionMode.Decompress))
                    {
                        using (var reader = new System.IO.StreamReader(zs, _mapping.Encode))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    using (var reader = new System.IO.StreamReader(respStream, _mapping.Encode))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }

            return result;
        }
    }
}
