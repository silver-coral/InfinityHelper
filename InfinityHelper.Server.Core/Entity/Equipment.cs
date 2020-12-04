using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public abstract class ItemBase
    {
        public int Bind { get; set; }
        [DisplayName("物理伤害")]
        public int? Attack { get; set; }
        public int? Coin { get; set; }
        public ItemColor Color { get; set; }
        [DisplayName("+ 闪避%")]
        public decimal? Evade { get; set; }
        [DisplayName("+ 命中率%")]
        public decimal? HitRate { get; set; }
        [DisplayName("+ 暴击伤害%")]
        public decimal? CriDamage { get; set; }
        [DisplayName("+ 暴击%")]
        public decimal? Critical { get; set; }
        public string Decs { get; set; }
        [DisplayName("+ 防御")]
        public int? Defense { get; set; }
        [DisplayName("+ 灵巧")]
        public int? Dexterous { get; set; }
        [DisplayName("等级")]
        public int Level { get; set; }
        [DisplayName("+ 生命")]
        public int? Life { get; set; }
        [DisplayName("法术伤害")]
        public int? MagAttack { get; set; }
        [DisplayName("+ 法力")]
        public int? Mana { get; set; }
        public int ItemNum { get; set; }
        public int MaxNum { get; set; }
        [DisplayName("+ 体格")]
        public int? Physique { get; set; }
        [DisplayName("+ 行动速度")]
        public decimal? Speed { get; set; }
        [DisplayName("+ 灵力")]
        public int? Spirit { get; set; }
        public string TypeDec { get; set; }

        public abstract string RealId { get; }
        public abstract string RealName { get; }
        public abstract ItemType RealType { get; }
        public abstract ItemKind RealKind { get; }
        public abstract int? RealCoin { get; }
        public abstract string RealDesc { get; }

        public string UniqueKey { get { return string.Format("{0}-{1}-{2}-{3}-{4}", 
            this.Level.ToString().PadLeft(4,'0'), 
            ((int)this.Color).ToString().PadLeft(2, '0'), this.RealType, this.RealKind, this.RealName); } }

        public ItemCategory RealCategory
        {
            get
            {
                if (RealType == ItemType.Equip)
                {
                    return (ItemCategory)RealKind;
                }
                else
                {
                    return (ItemCategory)((int)RealType * 100);
                }
            }
        }

        public bool IsStrenged()
        {
            return this.StrengAttack != null || this.StrengDefense != null || this.StrengDexterous != null || this.StrengLife != null
                || this.StrengMagAttack != null || this.StrengMana != null || this.StrengPhysique != null || this.StrengSpirit != null;
        }

        public bool IsEquip()
        {
            return this.RealCategory != ItemCategory.Material && this.RealCategory != ItemCategory.Item && this.RealCategory != ItemCategory.Skill;
        }

        public bool IsRequiredValid(int level)
        {
            return this.Level <= level;
        }

        public int? EnhanLevel { get; set; }
        public decimal? EnhanRange { get; set; }
        public int? StrengAttack { get; set; }
        public int? StrengDefense { get; set; }
        public int? StrengDexterous { get; set; }
        public int? StrengLife { get; set; }
        public int? StrengMagAttack { get; set; }
        public int? StrengMana { get; set; }
        public int? StrengPhysique { get; set; }
        public int? StrengSpirit { get; set; }
    }

    public class PackageItem : ItemBase
    {
        public string PackItemId { get; set; }
        public string ItemName { get; set; }
        public ItemType ItemType { get; set; }
        public ItemKind Kind { get; set; }
        public int? WhetherBatch { get; set; }

        public override string RealId => this.PackItemId;
        public override string RealName => this.EnhanLevel > 0 ? string.Format("{0}+{1}", this.ItemName, this.EnhanLevel) : this.ItemName;
        public override ItemType RealType => this.ItemType;
        public override ItemKind RealKind => this.Kind;
        public override int? RealCoin => this.Coin;
        public override string RealDesc => this.Decs;
    }

    public class BattleItem : ItemBase
    {
        public bool PickUp { get; set; }
        public int? ItemCoin { get; set; }
        public decimal? DropProb { get; set; }

        public string ItemName { get; set; }
        public ItemType? ItemType { get; set; }
        public ItemKind? Kind { get; set; }

        public override string RealId => this.UniqueKey;
        public override string RealName => this.ItemName;
        public override ItemType RealType => this.ItemType ?? Core.ItemType.Material;
        public override ItemKind RealKind => this.Kind ?? ItemKind.Weapon;
        public override int? RealCoin => this.ItemCoin;
        public override string RealDesc => this.Decs;
    }

    public class Equipment : ItemBase
    {
        public string EquitId { get; set; }
        public string EquitName { get; set; }
        public ItemType EquitType { get; set; }
        public ItemKind GeKind { get; set; }
        public string EquitDec { get; set; }

        public override string RealId => this.EquitId;
        public override string RealName => this.EnhanLevel > 0 ? string.Format("{0}+{1}", this.EquitName, this.EnhanLevel) : this.EquitName;
        public override ItemType RealType => this.EquitType;        
        public override ItemKind RealKind => this.GeKind;
        public override int? RealCoin => this.Coin;
        public override string RealDesc => this.EquitDec;
    }
}
