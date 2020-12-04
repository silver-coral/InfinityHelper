using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public abstract class InfinityApiBase
    {
        protected readonly InfinityServerSite _site;   
       
        protected IOwinRequest Request { get { return _site.Request; } }
        protected IOwinResponse Response { get { return _site.Response; } }

        public InfinityApiBase(InfinityServerSite site)
        {
            this._site = site;           
        }
        
        public string GetQuery(string key) { return _site.Uri.GetQuery(key); }
        public T GetQuery<T>(string key)
        {
            return _site.Uri.GetQuery<T>(key);
        }

        protected string GetPostString()
        {
            using (var reader = new System.IO.StreamReader(Request.Body))
            {
                return reader.ReadToEnd();
            }
        }

        protected T GetPostData<T>()
        {
            string postStr = GetPostString();
            return JsonUtil.Deserialize<T>(postStr);
        }

        public abstract void Execute();
    }
}
