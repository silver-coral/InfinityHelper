using InfinityHelper.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    [InfinityServerModel("help/mapdrop")]
    public class MapDropModel : InfinityServerModel
    {
        private ItemCategory? _cate;
        private string _mapId;

        public string CurrentMapId { get { return this._mapId; } }

        public string CurrentMapName
        {
            get
            {
                return string.IsNullOrEmpty(this._mapId) ? "全部" : this.MapList.FirstOrDefault(p => p.MapId == this._mapId).MapName;
            }
        }

        public int? CurrentCategory
        {
            get
            {
                return (int?)this._cate;
            }
        }       

        public string CurrentCategoryName
        {
            get
            {
                return this._cate == null ? "全部" : EnumUtil.GetDescription(this._cate);
            }
        }       

        public MapDropModel(InfinityServerSite site) : base(site)
        {
            Initialize();
        }

        public void Initialize()
        {
            this.Character = this._site.CurrentChar;
            InitMapItems();
        }               

        private void InitMapItems()
        {
            this.MapList = this._site.InitStaticAllMaps();
            

            this._cate = (ItemCategory?)GetQuery<int?>("type");
            this._mapId = GetQuery<string>("mid");                     
        }

        public Character Character { get; set; }
        public List<Map> MapList { get; set; }
    }
}
