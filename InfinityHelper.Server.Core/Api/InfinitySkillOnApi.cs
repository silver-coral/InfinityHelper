using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinitySkillOnApi : InfinityApiBase
    {
        public InfinitySkillOnApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            string sid = GetQuery("sid");
            int type = GetQuery<int>("type");
            this._site.SkillOn(sid, type);

            CharacterSkillCache.ClearCache(this._site.CurrentCharId);
            CharacterCache.ClearCache(this._site.CurrentCharId);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
