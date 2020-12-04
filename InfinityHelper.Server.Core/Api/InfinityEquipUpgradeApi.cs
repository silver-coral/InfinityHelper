using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityEquipUpgradeApi : InfinityApiBase
    {
        public InfinityEquipUpgradeApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            string eid = GetQuery("eid");
            this._site.EquipUpgrade(eid);

            CharacterCache.ClearCache(this._site.CurrentCharId);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
