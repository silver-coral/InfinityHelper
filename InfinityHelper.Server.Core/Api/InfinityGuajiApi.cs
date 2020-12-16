using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityGuajiApi : InfinityApiBase
    {
        public InfinityGuajiApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            if (string.IsNullOrEmpty(this._site.Config.CurrentMapId))
            {
                this._site.Config.CurrentMapId = this._site.InitAllSingleMaps().FirstOrDefault().MapId;
            }

            BattleScheduler.AddChar(this._site);

            this._site.Config.IsGuaji = true;
            CharacterConfigCache.SaveConfig(this._site.Config);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
