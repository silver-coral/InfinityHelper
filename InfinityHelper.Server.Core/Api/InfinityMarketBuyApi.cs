using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityMarketBuyApi : InfinityApiBase
    {
        public InfinityMarketBuyApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            string eid = GetQuery("eid");
            int count = GetQuery<int>("count");

            this._site.MarketBuy(eid, count);

            //CharacterCache.ClearCache(this._site.CurrentCharId);
            CharacterMarketCache.ClearCache(this._site.CurrentCharId);
            CharacterActivityCache.ClearCache(this._site.CurrentCharId);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
