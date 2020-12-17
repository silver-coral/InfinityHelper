using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityResetDungeonDynamicApi : InfinityApiBase
    {
        public InfinityResetDungeonDynamicApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            this._site.Dynamic.BattleDungeonLevelTotalCount = 0;
            this._site.Dynamic.BattleDungeonLevelTotalExp = 0;
            this._site.Dynamic.BattleDungeonLevelTotalWait = 0;
            this._site.Dynamic.BattleDungeonLevelWinCount = 0;
            CharacterDynamicCache.SaveDynamic(this._site.Dynamic);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
