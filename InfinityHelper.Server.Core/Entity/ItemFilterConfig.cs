using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class ItemFilterConfig
    {
        public ItemFilterConfig()
        {
            this.Filters = new List<ItemFilter>();
        }

        public string CharId { get; set; }        
        public List<ItemFilter> Filters { get; set; }
    }

    public class ItemFilter
    {
        public string Id { get; set; }
        public ItemColor? Color { get; set; }
        public ItemCategory? RealCategory { get; set; }
        public List<ItemFilterItem> Items { get; set; }
    }

    public class ItemFilterItem
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }       
        public decimal Value { get; set; }        
    }
}
