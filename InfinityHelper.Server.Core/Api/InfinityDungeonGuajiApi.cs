using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityDungeonGuajiApi : InfinityApiBase
    {
        public InfinityDungeonGuajiApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            if (string.IsNullOrEmpty(this._site.Config.CurrentDungeonMapId))
            {
                this._site.Config.CurrentDungeonMapId = this._site.InitAllDungeonMaps().FirstOrDefault().MapId;
            }

            BattleScheduler.AddChar(this._site, true);

            this._site.Config.IsDungeonGuaji = true;
            CharacterConfigCache.SaveConfig(this._site.Config);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
