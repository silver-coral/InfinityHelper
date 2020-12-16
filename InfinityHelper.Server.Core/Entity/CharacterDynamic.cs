using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class CharacterDynamic
    {
        public CharacterDynamic()
        {
            this.ItemList = new Dictionary<string, Dictionary<string, MapItem>>();
            this.DropItemsCount = new Dictionary<ItemColor, int>();
        }

        public string CharId { get; set; }

        public long TotalMoney { get; set; }
        public int BattleTotalCount { get; set; }
        public int BattleWinCount { get; set; }
        public long BattleTotalWait { get; set; }
        public long BattleTotalExp { get; set; }
       

        public int BattleLevelTotalCount { get; set; }
        public int BattleLevelWinCount { get; set; }
        public long BattleLevelTotalWait { get; set; }
        public long BattleLevelTotalExp { get; set; }


        public int BattleDungeonTotalCount { get; set; }
        public int BattleDungeonWinCount { get; set; }
        public int BattleDungeonLevelTotalCount { get; set; }
        public int BattleDungeonLevelWinCount { get; set; }
        public long BattleDungeonLevelTotalWait { get; set; }
        public long BattleDungeonLevelTotalExp { get; set; }


        public string TotalTimeStr
        {
            get
            {
                long totalTicks = BattleTotalWait * 10000;
                var ts = new TimeSpan(totalTicks);
                string totalTime = string.Format("{0}天{1}小时{2}分{3}秒", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
                return totalTime;
            }
        }
        public decimal TotalWinRate { get { return BattleTotalCount == 0 ? 0 : Math.Round(BattleWinCount * 100m / BattleTotalCount, 1); } }
        public decimal WinRate { get { return BattleLevelTotalCount == 0 ? 0 : Math.Round(BattleLevelWinCount * 100m / BattleLevelTotalCount, 1); } }
        public decimal EPM { get { return BattleLevelTotalWait == 0 ? 0 : Math.Round(BattleLevelTotalExp * 60 * 1000m / BattleLevelTotalWait); } }
        public DateTime? GetUpgradeTime(long exp)
        {
            DateTime? result = null;
            if (BattleLevelTotalExp > 0 && BattleLevelTotalWait > 0)
            {
                var upTicks = (long)Math.Ceiling(exp * 10000m / (BattleLevelTotalExp * 1m / BattleLevelTotalWait));
                result = DateTime.Now.AddTicks(upTicks);
            }
            return result;
        }

        public decimal DungeonTotalWinRate { get { return BattleDungeonTotalCount == 0 ? 0 : Math.Round(BattleDungeonWinCount * 100m / BattleDungeonTotalCount, 1); } }
        public decimal DungeonWinRate { get { return BattleDungeonLevelTotalCount == 0 ? 0 : Math.Round(BattleDungeonLevelWinCount * 100m / BattleDungeonLevelTotalCount, 1); } }
        public decimal DungeonEPM { get { return BattleDungeonLevelTotalWait == 0 ? 0 : Math.Round(BattleDungeonLevelTotalExp * 60 * 1000m / BattleDungeonLevelTotalWait); } }

        public Dictionary<ItemColor, int> DropItemsCount { get; set; }
        public Dictionary<string, Dictionary<string, MapItem>> ItemList { get; set; }

        public void AppendItem(string mapId, MapItem item)
        {
            if (!this.ItemList.ContainsKey(mapId))
            {
                this.ItemList[mapId] = new Dictionary<string, MapItem>();
            }

            this.ItemList[mapId][item.ItemId] = item;

            if (item.Color != null)
            {
                if (!this.DropItemsCount.ContainsKey(item.Color.Value))
                {
                    this.DropItemsCount[item.Color.Value] = 0;
                }
                this.DropItemsCount[item.Color.Value]++;
            }
        }
    }
}
