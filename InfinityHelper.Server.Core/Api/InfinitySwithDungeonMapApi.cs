using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinitySwithDungeonMapApi : InfinityApiBase
    {
        public InfinitySwithDungeonMapApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            this._site.Config.CurrentDungeonMapId = GetQuery("id");

            CharacterConfigCache.SaveConfig(this._site.Config);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
