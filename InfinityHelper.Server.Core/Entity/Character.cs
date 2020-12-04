using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class Character : ICharacter
    {
        public string AccountId { get; set; }
        public string Face { get; set; }
        public int Coin { get; set; }
        public decimal CriticalHitValue { get; set; }
        public decimal CritDamage { get; set; }
        public int Defense { get; set; }
        public int Dexterous { get; set; }
        public long Exp { get; set; }        
        public int Health { get; set; }        
        public string Id { get; set; }
        public int Level { get; set; }
        public int Lucky { get; set; }
        public int MagicAttack { get; set; }
        public int Mana { get; set; }
        public int MapId { get; set; }
        public long Money { get; set; }
        public decimal MovingSpeed { get; set; }        
        public string Name { get; set; }
        public int PackageNum { get; set; }
        public int PhysicalAttack { get; set; }
        public int Physique { get; set; }
        public int ReAttrPoint { get; set; }
        public int Spirit { get; set; }
        public int Status { get; set; }
        public decimal Evade { get; set; }
        public decimal HitRate { get; set; }
        public long UpgradeExp { get; set; }
        public string CharaId { get { return this.Id; } }
        public int RewardNum { get; set; }
        public int RealmExp { get; set; }
        public string RealmName { get; set; }
        public int RealmUpExp { get; set; }

        string ICharacter.Id => this.Id;

        string ICharacter.Name => this.Name;

        int ICharacter.Level => this.Level;

        int ICharacter.Life => this.Health;

        int ICharacter.Mana => this.Mana;

        [JsonIgnore]
        public bool IsGuaji { get; set; }

        [JsonIgnore]
        public decimal EPM { get; set; }

        [JsonIgnore]
        public decimal ExpPercent { get { return Math.Round(this.Exp * 100m / this.UpgradeExp, 1); } }

        [JsonIgnore]
        public decimal HitRatePercent { get { return Math.Round(this.HitRate * 100m, 1); } }

        [JsonIgnore]
        public decimal EvadePercent { get { return Math.Round(this.Evade * 100m, 1); } }

        [JsonIgnore]
        public decimal CriticalPercent { get { return Math.Round(this.CriticalHitValue * 100m, 1); } }

        [JsonIgnore]
        public decimal CriticalDamagePercent { get { return Math.Round(this.CritDamage * 100m, 1); } }
    }
}
