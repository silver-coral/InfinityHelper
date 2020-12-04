using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class Map
    {
        public Map()
        {
            this.ItemsVoList = new Dictionary<string, MapItem>();
        }

        public string MapId { get; set; }
        public string MapName { get; set; }
        public bool IsDungeon { get; set; }
        public int MinLv { get; set; }
        public int MaxLv { get; set; }

        public int MapMinLv { get { return this.MinLv; } }
        public int MapMaxLv { get { return this.MaxLv; } }
        public Dictionary<string,MapItem> ItemsVoList { get; set; }

        public List<MapItem> ItemList { get { return this.ItemsVoList.Values.OrderBy(p => p.OrderyKey).ToList(); } }
    }

    public class MapItem
    {
        public string ItemId { get; set; }
        public decimal DropProb { get; set; }
        public string ItemName { get; set; }
        public ItemColor? Color { get; set; }
        public int? Level { get; set; }
        public string TypeDec { get; set; }
        public ItemCategory? RealCategory { get; set; }

        public string OrderyKey
        {
            get
            {
                return string.Format("{0}-{1}-{2}", ((int)this.Color).ToString().PadLeft(2, '0'), this.Level.ToString().PadLeft(4, '0'), this.ItemName);
            }
        }

        public decimal DropPercent { get { return this.DropProb * 100m; } }

        public static MapItem FromItem(BattleItem item)
        {
            return new MapItem()
            {
                ItemId = item.UniqueKey,
                Color = item.Color,
                ItemName = item.RealName,
                Level = item.Level,
                TypeDec = item.TypeDec,
                RealCategory = item.RealCategory,
                DropProb = item.DropProb ?? 0,
            };
        }
    }
}
