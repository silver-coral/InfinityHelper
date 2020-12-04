using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityEquipUnBindApi : InfinityApiBase
    {
        public InfinityEquipUnBindApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            string eid = GetQuery("eid");
            this._site.EquipUnBind(eid);

            CharacterEquipCache.ClearCache(this._site.CurrentCharId);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
