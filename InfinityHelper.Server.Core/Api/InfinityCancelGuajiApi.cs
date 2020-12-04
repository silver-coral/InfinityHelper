using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityCancelGuajiApi : InfinityApiBase
    {
        public InfinityCancelGuajiApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            BattleScheduler.CancelChar(this._site.CurrentCharId);

            this._site.Config.IsGuaji = false;
            CharacterConfigCache.SaveConfig(this._site.Config);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
