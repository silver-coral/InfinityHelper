using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityClearGlobalCacheApi : InfinityApiBase
    {
        public InfinityClearGlobalCacheApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {                   
            AllMapCache.ClearCache("0");
            AllSyntheticCache.ClearCache("0");

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
