using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinitySkillUpgradeApi : InfinityApiBase
    {
        public InfinitySkillUpgradeApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            string eid = GetQuery("sid");
            this._site.SkillUpgrade(eid);

            CharacterCache.ClearCache(this._site.CurrentCharId);
            CharacterActivityCache.ClearCache(this._site.CurrentCharId);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
