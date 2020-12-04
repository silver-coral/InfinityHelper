using InfinityHelper.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    [InfinityServerModel("skill/bag")]
    public class SkillBagModel : InfinityServerModel
    {
        private SkillType? _type;

        public int? CurrentType
        {
            get
            {
                return (int?)this._type;
            }
        }

        public string CurrentTypeName
        {
            get
            {
                return this._type == null ? "全部" : EnumUtil.GetDescription(this._type);
            }
        }

        public SkillBagModel(InfinityServerSite site) : base(site)
        {
            Initialize();
        }

        public void Initialize()
        {
            this.AllSkillList = this._site.InitCharSkills();

            InitBagSkills();
        }        

        private void InitBagSkills()
        {
            string path = string.Format("/foodie-api/gameCharaSkill/getCharaSkill?charaId={0}", this._site.CurrentCharId);
            this.BagSkillList = this._site.PostResult<List<Skill>>(path, null);

            this._type = (SkillType?)GetQuery<int?>("type");
            if (this._type != null)
            {
                this.BagSkillList = this.BagSkillList.Where(p => p.SkillType == this._type).ToList();
            }
        }

        public List<Skill> AllSkillList { get; set; }
        public List<Skill> SkillList { get { return this.AllSkillList.Where(p => !p.IsPassive).ToList(); } }
        public List<Skill> PassiveSkillList { get { return this.AllSkillList.Where(p => p.IsPassive).ToList(); } }
        public List<Skill> BagSkillList { get; set; }
    }
}
