using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class Skill
    {
        public int? ConMana { get; set; }
        public int? ConsumeCoin { get; set; }
        public string EquipPage { get; set; }
        public string SkillDesc { get; set; }
        public string SkillIcon { get; set; }
        public string SkillId { get; set; }
        public string SkillName { get; set; }
        public string SkillNextDesc { get; set; }
        public SkillType SkillType { get; set; }

        public bool IsPassive { get { return SkillType == SkillType.Passive; } }
        public bool IsPhysical { get { return SkillType == SkillType.Physical; } }
        public bool IsMagical { get { return SkillType == SkillType.Magical; } }
    }
}
