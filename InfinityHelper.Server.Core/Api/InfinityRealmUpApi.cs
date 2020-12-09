using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityRealmUpApi : InfinityApiBase
    {
        public InfinityRealmUpApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {            
            this._site.RealmUp();

            CharacterCache.ClearCache(this._site.CurrentCharId);
            RealmBonusCache.ClearCache(this._site.CurrentCharId);
            CharacterActivityCache.ClearCache(this._site.CurrentCharId);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
