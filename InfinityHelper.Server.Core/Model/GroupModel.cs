using InfinityHelper.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    [InfinityServerModel("character/group")]
    public class GroupModel : InfinityServerModel
    {
        public GroupModel(InfinityServerSite site) : base(site)
        {
            Initialize();
        }

        public void Initialize()
        {
            this.Character = this._site.CurrentChar;            
            this.ArmyGroup = this._site.InitArmyGroup();
            this.ArmyGroupList = this._site.QueryAllArmyGroups();

            if (this.HasGroup)
            {
                var c = this.ArmyGroup.CharaInfoVoList.FirstOrDefault(p => p.IsCaption);
                if (c != null)
                {
                    this.IsCaption = c.CharaNo == this.Character.AccountId;
                }
            }
        }                        

        public Character Character { get; set; }        
        public ArmyGroup ArmyGroup { get; set; }       
        public List<ArmyGroup> ArmyGroupList { get; set; }
        public bool HasGroup { get { return this.ArmyGroup != null; } }
        public bool IsCaption { get; set; }
    }
}
