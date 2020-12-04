using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class ArmyGroup
    {
        public string ArmyId { get; set; }
        public string ArmyName { get; set; }
        public int ArmyNum { get; set; }
        public string CaptainName { get; set; }
        public string CaptainNo { get; set; }
        public string CaptionId { get; set; }
        public List<ArmyCharInfo> CharaInfoVoList { get; set; }
    }

    public class ArmyCharInfo
    {
        public string Avatar { get; set; }
        public int CharaLevel { get; set; }
        public string CharaName { get; set; }
        public string CharaNo { get; set; }
        public int Status { get; set; }
        public bool IsCaption { get { return this.Status == 1; } }
    }
}
