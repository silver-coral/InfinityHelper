using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityExchangeCodeApi : InfinityApiBase
    {
        public InfinityExchangeCodeApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            string code = GetQuery("code");
            this._site.ExchangeCode(code);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
