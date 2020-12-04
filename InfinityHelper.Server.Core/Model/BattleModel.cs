using InfinityHelper.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    [InfinityServerModel("map/battle")]
    public class BattleModel : InfinityServerModel
    {
        public BattleModel(InfinityServerSite site) : base(site)
        {
            Initialize();
        }

        public void Initialize()
        {
            this.Index = GetQuery<int>("i");

            this.Character = this._site.CurrentChar;

            if (BattleScheduler.ResultDic.ContainsKey(this._site.CurrentCharId))
            {
                var currentList = BattleScheduler.ResultDic[this._site.CurrentCharId];
                if (this.Index >= 0 && this.Index < currentList.Count)
                {
                    this.Battle = currentList[this.Index];
                    this.Count = currentList.Count;
                }                
            }
        }                       

        public Character Character { get; set; }        
        public BattleResult Battle { get; set; }
        public int Count { get; set; }
        public int Index { get; set; }
    }
}
