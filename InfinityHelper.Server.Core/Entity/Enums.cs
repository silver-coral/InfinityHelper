using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{

    public enum ItemCategory
    {
        [Description("物理武器")]
        Weapon = 1,
        [Description("灵力武器")]
        MagicalWeapon = 200,
        [Description("衣服")]
        Armor = 2,
        [Description("头盔")]
        Helm = 3,
        [Description("项链")]
        Amulet = 4,
        [Description("戒指")]
        Ring = 7,
        [Description("腰带")]
        Belt = 9,
        [Description("鞋子")]
        Boot = 10,        
        [Description("道具")]
        Item = 300,
        [Description("材料")]
        Material = 400,
        [Description("秘籍书")]
        Skill = 600,
    }

    public enum ItemType
    {        
        Equip = 1,
        MagicalWeapon = 2,
        Item = 3,
        Material = 4,
    }

    public enum ItemKind
    {
        Weapon = 1,
        Armor = 2,
        Helm = 3,
        Amulet = 4,
        Ring = 7,
        Belt = 9,
        Boot = 10,
    }

    public enum BattleHurtType
    {
        PhysicalDamage = 1,
        MagicalDamage = 2,
        DotDamage = 3,
        CounterDamage = 4,
        Heal = 5,
    }

    public enum ItemColor
    {
        [Description("黑色")]
        Black = 1,
        [Description("黄色")]
        Yellow = 2,
        [Description("绿色")]
        Green = 3,
        [Description("蓝色")]
        Blue = 4,
        [Description("紫色")]
        Pink = 5,
        [Description("红色")]
        Red = 6,
    }

    public enum SkillType
    {
        [Description("法术")]
        Magical = 0,
        [Description("物理")]
        Physical = 1,        
        [Description("被动")]
        Passive = 2,
    }
}
