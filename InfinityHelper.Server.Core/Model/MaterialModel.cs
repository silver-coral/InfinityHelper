using InfinityHelper.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    [InfinityServerModel("equip/material")]
    public class MaterialModel : InfinityServerModel
    {
        public MaterialModel(InfinityServerSite site) : base(site)
        {
            Initialize();
        }

        public void Initialize()
        {
            this.Character = this._site.CurrentChar;
            this.MarketItems = this._site.InitMarket();
            this.SyntheticList = this._site.InitAllSynthetics();

            InitBagItems();
        }               

        private void InitBagItems()
        {
            this.BagItemList = this._site.QueryBagItems();            
        }

        public Character Character { get; set; }
        public List<Synthetic> SyntheticList { get; set; }
        public List<MarketItem> MarketItems { get; set; }
        public List<PackageItem> BagItemList { get; set; }
        public List<PackageItem> BagItemItemList { get { return this.BagItemList.Where(p => p.RealCategory == ItemCategory.Item).ToList(); } }
        public List<PackageItem> BagMaterialItemList { get { return this.BagItemList.Where(p => p.RealCategory == ItemCategory.Material).ToList(); } }
    }
}
