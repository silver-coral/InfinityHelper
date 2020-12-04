using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinitySwithMapApi : InfinityApiBase
    {
        public InfinitySwithMapApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            this._site.Config.CurrentMapId = GetQuery("id");

            CharacterConfigCache.SaveConfig(this._site.Config);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
