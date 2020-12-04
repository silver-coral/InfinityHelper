using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class RealmBonus
    {
        public decimal? RealmAttack { get; set; }
        public decimal? RealmCriticalDamage { get; set; }
        public decimal? RealmCriticalHit { get; set; }
        public decimal? RealmDefense { get; set; }
        public decimal? RealmEvade { get; set; }
        public decimal? RealmHealth { get; set; }
        public decimal? RealmHitRate { get; set; }
        public decimal? RealmMagicAttack { get; set; }
        public decimal? RealmMana { get; set; }
        public string RealmName { get; set; }
        public decimal? RealmSpeed { get; set; }

        [JsonIgnore]
        public decimal AttackPercent { get { return Math.Round((this.RealmAttack ?? 0) * 100m, 1); } }

        [JsonIgnore]
        public decimal DefensePercent { get { return Math.Round((this.RealmDefense ?? 0) * 100m, 1); } }

        [JsonIgnore]
        public decimal HealthPercent { get { return Math.Round((this.RealmHealth ?? 0) * 100m, 1); } }

        [JsonIgnore]
        public decimal MagicAttackPercent { get { return Math.Round((this.RealmMagicAttack ?? 0) * 100m, 1); } }

        [JsonIgnore]
        public decimal ManaPercent { get { return Math.Round((this.RealmMana ?? 0) * 100m, 1); } }

        [JsonIgnore]
        public decimal SpeedPercent { get { return Math.Round((this.RealmSpeed ?? 0) * 100m, 1); } }

        [JsonIgnore]
        public decimal HitRatePercent { get { return Math.Round((this.RealmHitRate ?? 0) * 100m, 1); } }

        [JsonIgnore]
        public decimal EvadePercent { get { return Math.Round((this.RealmEvade ?? 0) * 100m, 1); } }

        [JsonIgnore]
        public decimal CriticalPercent { get { return Math.Round((this.RealmCriticalHit ?? 0) * 100m, 1); } }

        [JsonIgnore]
        public decimal CriticalDamagePercent { get { return Math.Round((this.RealmCriticalDamage ?? 0) * 100m, 1); } }
    }
}
