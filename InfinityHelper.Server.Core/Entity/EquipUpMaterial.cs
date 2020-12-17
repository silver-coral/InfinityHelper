using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class EquipUpMaterial
    {
        public int? Coin { get; set; }
        public int? ConinType { get; set; }
        public string ExtraRange { get; set; }
        public string MaterOne { get; set; }
        public string MaterOneName { get; set; }
        public int? MaterOneNum { get; set; }
        public string MaterRange { get; set; }
        public string MaterTwo { get; set; }
        public string MaterTwoName { get; set; }
        public int? MaterTwoNum { get; set; }
        public string MaxRange { get; set; }
        public string MinRange { get; set; }
        public int MinStrengLevel { get; set; }

        public string PriceTypeStr { get { return ConinType == null ? "无" : ConinType == 0 ? "铜币" : "金币"; } }
    }
}
