using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityFilterRemoveApi : InfinityApiBase
    {
        public InfinityFilterRemoveApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            var id = this.GetQuery<string>("id");

            var filter = this._site.FilterConfig.Filters.FirstOrDefault(p => p.Id == id);
            if(filter != null)
            {
                this._site.FilterConfig.Filters.Remove(filter);
                ItemFilterCache.SaveConfig(this._site.FilterConfig);
            }            

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
