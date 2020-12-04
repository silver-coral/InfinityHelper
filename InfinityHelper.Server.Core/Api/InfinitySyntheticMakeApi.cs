using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinitySyntheticMakeApi : InfinityApiBase
    {
        public InfinitySyntheticMakeApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            string sid = GetQuery("sid");
            int count = GetQuery<int>("count");
            this._site.SyntheticMake(sid, count);

            CharacterCache.ClearCache(this._site.CurrentCharId);            

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
