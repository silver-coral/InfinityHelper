using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityFilterClearApi : InfinityApiBase
    {
        public InfinityFilterClearApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            this._site.FilterConfig.Filters.Clear();
            ItemFilterCache.SaveConfig(this._site.FilterConfig);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
