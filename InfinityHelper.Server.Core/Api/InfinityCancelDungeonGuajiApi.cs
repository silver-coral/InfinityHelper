using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityCancelDungeonGuajiApi : InfinityApiBase
    {
        public InfinityCancelDungeonGuajiApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            BattleScheduler.CancelChar(this._site.CurrentCharId);

            this._site.Config.IsDungeonGuaji = false;
            CharacterConfigCache.SaveConfig(this._site.Config);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
