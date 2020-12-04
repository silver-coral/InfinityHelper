using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class ProxyUri
    {
        private Uri _rawUri;

        private static string _imgExtFormats = ".jpg.jpeg.png.peng.gif.bmp.tiff.svg.pcx.psd.eps.ico";

        private static string _jsExtFormats = ".js";

        private static string _cssExtFormats = ".css";

        public bool IsStatic()
        {
            return this.PathAndQuery.Contains(".");
            //return IsJs() || IsCss() || IsImage();
        }

        public bool IsJs()
        {
            string fileExtension = Path.GetExtension(AbsolutePath);
            return !string.IsNullOrEmpty(fileExtension) && _jsExtFormats.Contains(fileExtension);
        }

        public bool IsCss()
        {
            string fileExtension = Path.GetExtension(AbsolutePath);
            return !string.IsNullOrEmpty(fileExtension) && _cssExtFormats.Contains(fileExtension);
        }

        public bool IsImage()
        {
            string fileExtension = Path.GetExtension(AbsolutePath);
            return !string.IsNullOrEmpty(fileExtension) && _imgExtFormats.Contains(fileExtension);
        }

        public bool IsLogin()
        {
            return this.PathAndQuery.ToLower().Contains("home/login");
        }

        public bool IsRoot()
        {
            return this.PathAndQuery.ToLower() == "/";
        }

        public bool IsApi()
        {
            return this.PathAndQuery.ToLower().Contains("api/");
        }

        /// <summary>
        /// http://{host}:{port}
        /// </summary>
        public string Authority
        {
            get
            {
                string result = _rawUri.GetLeftPart(UriPartial.Authority);
                return result;
            }
        }

        /// <summary>
        /// /{name}    => error
        /// /{name}/
        /// /{name}/path1
        /// /{name}/path1/path2
        /// </summary>
        public string AbsolutePath
        {
            get
            {
                return _rawUri.AbsolutePath;
            }
        }

        /// <summary>
        /// /{name}?key1=value1    => error
        /// /{name}/?key1=value1
        /// /{name}/path1?key1=value1
        /// /{name}/path1/path2?key1=value1&key2=value2
        /// </summary>
        public string PathAndQuery
        {
            get
            {
                return _rawUri.PathAndQuery;
            }
        }
        

        public string MappingName
        {
            get
            {
                return string.Empty;
            }
        }

        public string ApiName
        {
            get
            {
                string result = AbsolutePath;
                result = result.TrimStart('/').TrimEnd('/');
                return result.ToLower();
            }
        }

        /// <summary>
        /// /{name}  => error
        /// /{name}/ => _index
        /// /{name}/path1  => path1
        /// /{name}/path1/path2 => path1/path2
        /// </summary>
        public string TemplateName
        {
            get
            {
                string result = AbsolutePath;
                result = result.TrimStart('/').TrimEnd('/');
                return result.ToLower();
            }
        }

        private Dictionary<string, string> _query;

        public Dictionary<string, string> Query { get { return _query; } }

        public string GetQuery(string key)
        {
            if (Query.ContainsKey(key))
            {
                return Query[key];
            }
            return string.Empty;
        }

        public T GetQuery<T>(string key)
        {
            string str = GetQuery(key);
            if (string.IsNullOrWhiteSpace(str))
            {
                return default(T);
            }
            Type type = typeof(T);
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var paramType = type.GetGenericArguments()[0];
                return (T)Convert.ChangeType(str, paramType);
            }
            else
            {
                return (T)Convert.ChangeType(str, type);
            }
        }


        public ProxyUri(IOwinRequest _request)
        {
            this._rawUri = _request.Uri;
            this._query = _request.Query.ToDictionary(p => p.Key, p => p.Value.FirstOrDefault());
        }

        public void UpdateUri(Uri uri)
        {
            this._rawUri = uri;
        }
    }
}
