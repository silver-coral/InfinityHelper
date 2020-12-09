using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityClearCacheApi : InfinityApiBase
    {
        public InfinityClearCacheApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            CharacterCache.ClearCache(this._site.CurrentCharId);
            CharacterEquipCache.ClearCache(this._site.CurrentCharId);
            CharacterSkillCache.ClearCache(this._site.CurrentCharId);            
            AllMapCache.ClearCache(this._site.CurrentCharId);
            CharacterMarketCache.ClearCache(this._site.CurrentCharId);
            CharacterArmyGroupCache.ClearCache(this._site.CurrentCharId);
            RealmBonusCache.ClearCache(this._site.CurrentCharId);
            CharacterActivityCache.ClearCache(this._site.CurrentCharId);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
