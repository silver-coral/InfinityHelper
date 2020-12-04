using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityAttrsUpdateApi : InfinityApiBase
    {
        public InfinityAttrsUpdateApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            int csa = GetQuery<int>("csa");
            int cda = GetQuery<int>("cda");
            int cea = GetQuery<int>("cea");
            this._site.AttrsUpdate(csa, cda, cea);

            CharacterCache.ClearCache(this._site.CurrentCharId);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
