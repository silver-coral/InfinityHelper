using InfinityHelper.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    [InfinityServerModel("equip/bag")]
    public class BagModel : InfinityServerModel
    {
        private ItemCategory? _cate;
        private ItemColor? _color;

        public int? CurrentCategory
        {
            get
            {
                return (int?)this._cate;
            }
        }

        public int? CurrentColor
        {
            get
            {
                return (int?)this._color;
            }
        }

        public string CurrentCategoryName
        {
            get
            {
                return this._cate == null ? "全部" : EnumUtil.GetDescription(this._cate);
            }
        }

        public string CurrentColorName
        {
            get
            {
                return this._color == null ? "全部" : EnumUtil.GetDescription(this._color);
            }
        }

        public BagModel(InfinityServerSite site) : base(site)
        {
            Initialize();
        }

        public void Initialize()
        {
            this.Character = this._site.CurrentChar;
            this.CharEquipList = this._site.InitCharEquips();

            InitBagItems();
        }               

        private void InitBagItems()
        {
            this.BagItemList = this._site.QueryBagItems();

            this._cate = (ItemCategory?)GetQuery<int?>("type");
            this._color = (ItemColor?)GetQuery<int?>("color");
            if (this._cate != null)
            {
                this.BagItemList = this.BagItemList.Where(p => p.RealCategory == this._cate).ToList();
            }
            if(this._color != null)
            {
                this.BagItemList = this.BagItemList.Where(p => p.Color == this._color).ToList();
            }

            this.BagItemList = this.BagItemList.Where(p => p.RealCategory != ItemCategory.Item && p.RealCategory != ItemCategory.Material).ToList();
        }

        public Character Character { get; set; }
        public List<Equipment> CharEquipList { get; set; }
        public List<PackageItem> BagItemList { get; set; }
        public List<PackageItem> BagBindItemList { get { return this.BagItemList.Where(p => p.Bind == 1).ToList(); } }
        public List<PackageItem> BagUnBindItemList { get { return this.BagItemList.Where(p => p.Bind == 0).ToList(); } }
    }
}
