using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class RealmUpMaterial
    {       
        public string MaterOne { get; set; }
        public string MaterOneName { get; set; }
        public int? MaterOneNum { get; set; }        
        public string MaterTwo { get; set; }
        public string MaterTwoName { get; set; }
        public int? MaterTwoNum { get; set; }
        public string MaterThree { get; set; }
        public string MaterThreeName { get; set; }
        public int? MaterThreeNum { get; set; }

        public string RealmId { get; set; }
        public int RealmLevel { get; set; }
        public int RealmMaxLevel { get; set; }
        public string RealmName { get; set; }
        public int UpdateExp { get; set; }        
    }
}
