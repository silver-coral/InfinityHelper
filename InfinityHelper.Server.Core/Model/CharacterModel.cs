using InfinityHelper.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    [InfinityServerModel("character/detail")]
    public class CharacterDetailModel : InfinityServerModel
    {
        private string charId;

        public CharacterDetailModel(InfinityServerSite site) : base(site)
        {
            Initialize();
        }

        private void LoadFromCurrentChar()
        {
            this.Character = this._site.CurrentChar;
            this.CharacterDynamic = this._site.Dynamic;
            this.CharEquipList = this._site.InitCharEquips();
            this.AllSkillList = this._site.InitCharSkills();
            this.MapList = this._site.InitAllMaps();
            //this.MarketItems = this._site.InitMarket();
            this.ArmyGroup = this._site.InitArmyGroup();
            this.RealmBonus = this._site.InitRealmBonus();

            this.CurrentMap = this.MapList.FirstOrDefault(p => p.MapId == this._site.Config.CurrentMapId);

            if (this.CurrentMap == null)
            {
                this.CurrentMap = this.MapList.FirstOrDefault();

                this.Site.Config.CurrentMapId = this.CurrentMap.MapId;
                CharacterConfigCache.SaveConfig(this.Site.Config);
            }
        }

        public void Initialize()
        {
            this.charId = GetQuery<string>("id");

            if(!string.IsNullOrEmpty(this.charId) && this.charId != this._site.CurrentCharId)
            {
                this._site.CurrentCharId = this.charId;
                this._site.Config = CharacterConfigCache.TryGetValue(this.charId, CharacterConfigCache.LoadConfig);
                this._site.Dynamic = CharacterDynamicCache.TryGetValue(this.charId, CharacterDynamicCache.LoadDynamic);
                this.IsReadOnly = true;
            }

            LoadFromCurrentChar();
        }                        

        public List<Skill> AllSkillList { get; set; }
        public List<Skill> SkillList { get { return this.AllSkillList.Where(p => !p.IsPassive).ToList(); } }
        public List<Skill> PassiveSkillList { get { return this.AllSkillList.Where(p => p.IsPassive).ToList(); } }
        public Character Character { get; set; }
        public CharacterDynamic CharacterDynamic { get; set; }
        public List<Equipment> CharEquipList { get; set; }        
        public List<Map> MapList { get; set; }
        public Map CurrentMap { get; set; }
        //public List<MarketItem> MarketItems { get; set; }
        public ArmyGroup ArmyGroup { get; set; }
        public bool IsReadOnly { get; set; }
        public bool HasGroup { get { return this.ArmyGroup != null; } }
        public RealmBonus RealmBonus { get; set; }
    }
}
