using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityFilterCreateApi : InfinityApiBase
    {
        public InfinityFilterCreateApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            var filter = this.GetPostData<ItemFilter>();
            filter.Id = Guid.NewGuid().ToString();
            foreach(var item in filter.Items)
            {
                item.DisplayName = ItemFilterHelper.EquipFilterDict[item.Name];
            }

            this._site.FilterConfig.Filters.Add(filter);
            ItemFilterCache.SaveConfig(this._site.FilterConfig);            

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
