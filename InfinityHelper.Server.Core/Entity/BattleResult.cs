using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class BattleResult
    {
        public bool? Success { get; set; }
        public int DropExp { get; set; }
        public int ReHealth { get; set; }
        public int ReMana { get; set; }
        public bool? UpgradeLev { get; set; }
        public List<BattleItem> GameItemsList { get; set; }
        public List<BattleTurn> CombatInfo { get; set; }
        public List<Monster> GameMonList { get; set; }

        public Dictionary<string, int> LifeBefore { get; set; }
        public Dictionary<string, int> LifeAfter { get; set; }

        public IEnumerable<ICharacter> LeftList { get; set; }
        public IEnumerable<ICharacter> RightList { get { return this.GameMonList; } }

        public decimal WaitSecond { get { return this.Wait / 1000m; } }
        public int Wait { get { return this.CombatInfo.Count * 1500 + 5000; } }  
        public int TotalCount { get; set; }
        public decimal EPM { get; set; }

        public Dictionary<string,BattleStatisticsData> Statistics { get; set; }
    }

    public class BattleTurn
    {
        public string Attacker { get; set; }
        public string Hurt { get; set; }
        public BattleHurtType? HurtType { get; set; }
        public string Hinjured { get; set; }
        public int? IsCritical { get; set; }
        public int? IsDeath { get; set; }
        public int? IsSkill { get; set; }
        public string SkillName { get; set; }
        public int Identity { get; set; }
        public int? DropExp { get; set; }
        public int SurplusHealth { get; set; }

        public int CurrentTurn { get; set; }        
        public List<BattleTurnHp> HpList { get; set; }

        public List<BattleTurn> HinjuredConmbatList { get; set; }
    }

    public class BattleTurnHp
    {
        public string id { get; set; }
        public int chp { get; set; }
        public int hp { get; set; }
        public int cmp { get; set; }
        public int mp { get; set; }
    }

    public class BattleStatisticsData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int DirectDamage { get { return this.NormalDamage + this.PhysicalDamage + this.MagicalDamage; } }
        public int NormalDamage { get; set; }
        public int PhysicalDamage { get; set; }
        public int MagicalDamage { get; set; }
        public int CounterDamage { get; set; }
        public int DotDamage { get; set; }
        public int Heal { get; set; }
        public int TotalDamage { get { return this.DirectDamage + this.CounterDamage + this.DotDamage; } }
    }

    public enum BattleDamageType
    {
        Direct,Counter,Dot
    }
}
