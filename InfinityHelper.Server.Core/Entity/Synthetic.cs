using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class Synthetic
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

        public int? ItemId { get; set; }
        public int SyntheticId { get; set; }
        public int? SyntheticExp { get; set; }
        public int? SyntheticMoney { get; set; }
        public string SyntheticDesci { get; set; }
        public string SyntheticName { get; set; }
        public string SyntheticRealm { get; set; }
        public string SyntheticRealmId { get; set; }
    }
}
