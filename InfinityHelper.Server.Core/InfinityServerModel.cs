using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityServerModel
    {       
        protected ProxyUri _uri;
        protected InfinityServerSite _site;
      
        public string CurrentCharId { get { return _site.CurrentCharId; } }

        public InfinityServerModel(InfinityServerSite site)
        {            
            this._uri = site.Uri;
            this._site = site;           
        }

        public InfinityServerSite Site { get { return _site; } }

        public string GetQuery(string key) { return _uri.GetQuery(key); }
        public T GetQuery<T>(string key)
        {
            return _uri.GetQuery<T>(key);
        }

        public string ProxyAuthority { get { return _uri.Authority; } }        
    }
}
