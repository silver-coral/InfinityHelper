using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityResetDynamicApi : InfinityApiBase
    {
        public InfinityResetDynamicApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            this._site.Dynamic.BattleLevelTotalCount = 0;
            this._site.Dynamic.BattleLevelTotalExp = 0;
            this._site.Dynamic.BattleLevelTotalWait = 0;
            this._site.Dynamic.BattleLevelWinCount = 0;
            CharacterDynamicCache.SaveDynamic(this._site.Dynamic);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
