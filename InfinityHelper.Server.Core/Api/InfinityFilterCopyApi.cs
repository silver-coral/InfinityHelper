using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityFilterCopyApi : InfinityApiBase
    {
        public InfinityFilterCopyApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            string name = this.GetQuery<string>("name");

            var c = CharacterCache.GetAll().FirstOrDefault(p => p.Name == name);
            if (c != null)
            {
                if (ItemFilterCache.HasCache(c.Id))
                {
                    var cfg = ItemFilterCache.LoadCache(c.Id);

                    this._site.FilterConfig.Filters.Clear();

                    foreach(var p in cfg.Filters)
                    {
                        var newFilter = new ItemFilter()
                        {
                            Color = p.Color,
                            RealCategory = p.RealCategory,
                            Id = Guid.NewGuid().ToString(),
                            Items = new List<ItemFilterItem>(),
                        };
                        foreach(var item in p.Items)
                        {
                            newFilter.Items.Add(new ItemFilterItem
                            {
                                DisplayName = item.DisplayName,
                                Name = item.Name,
                                Value = item.Value,
                            });
                        }

                        this._site.FilterConfig.Filters.Add(newFilter);
                    }
                                     
                    ItemFilterCache.SaveConfig(this._site.FilterConfig);                   
                }
            }

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
