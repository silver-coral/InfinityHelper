using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityEquipBindApi : InfinityApiBase
    {
        public InfinityEquipBindApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            string eid = GetQuery("eid");
            this._site.EquipBind(eid);            

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
