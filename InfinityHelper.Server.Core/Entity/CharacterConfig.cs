using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class CharacterConfig
    {
        public CharacterConfig()
        {
            this.IPList = new List<string>();
        }

        public string CharId { get; set; }
        public string CurrentMapId { get; set; }   
        public string CurrentDungeonMapId { get; set; }
        public bool IsGuaji { get; set; }
        public bool IsDungeonGuaji { get; set; }
        public List<string> IPList { get; set; }
    }
}
