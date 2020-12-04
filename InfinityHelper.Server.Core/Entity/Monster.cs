using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public interface ICharacter
    {
        string Id { get; }
        string Name { get; }
        int Level { get; }
        int Life { get; }        
        int Mana { get; }       
    }

    public class Monster : ICharacter
    {
        public string MonsterId { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public int Level { get; set; }
        public int Life { get; set; }
        public int Mana { get; set; }
        public int AttackMax { get; set; }
        public int AttackMin { get; set; }
        public int Defence { get; set; }
        public decimal Explode { get; set; }
        public decimal CriDamage { get; set; }
        public decimal HitRate { get; set; }
        public decimal Evade { get; set; }
        public decimal Speed { get; set; }
        public int? Spirit { get; set; }
        public int? Dexterity { get; set; }
        public int? Physique { get; set; }
        public int DropExp { get; set; }
        public decimal Probility { get; set; }
        public string MonPrifex { get; set; }

        public string BattleCharId { get; set; }

        string ICharacter.Id => this.BattleCharId;

        string ICharacter.Name => this.Name;

        int ICharacter.Level => this.Level;

        int ICharacter.Life => this.Life;

        int ICharacter.Mana => this.Mana;        
    }
}
