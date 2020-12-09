using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityEquipSellApi : InfinityApiBase
    {
        public InfinityEquipSellApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            string eids = GetQuery("eids");
            this._site.EquipSell(eids);

            //CharacterCache.ClearCache(this._site.CurrentCharId);
            CharacterActivityCache.ClearCache(this._site.CurrentCharId);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
