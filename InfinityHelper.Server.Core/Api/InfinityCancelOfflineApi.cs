using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityCancelOfflineApi : InfinityApiBase
    {
        public InfinityCancelOfflineApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            var result = this._site.EndOffline();
            this._site.AutoSell();

            CharacterCache.ClearCache(this._site.CurrentCharId);
            CharacterActivityCache.ClearCache(this._site.CurrentCharId);

            Response.WriteAsync(JsonUtil.Serialize(result));
        }
    }
}
