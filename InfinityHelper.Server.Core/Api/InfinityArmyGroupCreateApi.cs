using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityArmyGroupCreateApi : InfinityApiBase
    {
        public InfinityArmyGroupCreateApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            string name = GetQuery<string>("name");

            this._site.ArmyGroupCreate(name);

            CharacterArmyGroupCache.ClearCache(this._site.CurrentCharId);
            AllMapCache.ClearCache(this._site.CurrentCharId);            
            CharacterConfigCache.ClearState(this._site.Config);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
