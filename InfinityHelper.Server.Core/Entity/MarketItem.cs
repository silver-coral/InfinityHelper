using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class MarketItem
    {
        public string MarketId { get; set; }
        public int MarketLevel { get; set; }
        public string MarketName { get; set; }
        public int MarketNum { get; set; }
        public int MarketPrice { get; set; }
        public int MarketRefresh { get; set; }
        public int MarketType { get; set; }
        public int PriceType { get; set; }
        public string MarketDec { get; set; }

        public string PriceTypeStr { get { return PriceType == 0 ? "铜币" : "金币"; } }
    }
}
