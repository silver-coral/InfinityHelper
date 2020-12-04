using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityArmyGroupLeaveApi : InfinityApiBase
    {
        public InfinityArmyGroupLeaveApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            var army = this._site.InitArmyGroup();
            if (army != null)
            {
                bool isCaption = this._site.CheckIsGroupCaption();

                this._site.ArmyGroupLeave();

                foreach(var c in army.CharaInfoVoList)
                {
                    var gc = CharacterCache.GetCharByNo(c.CharaNo);
                    if(gc != null)
                    {
                        CharacterArmyGroupCache.ClearCache(gc.CharaId);
                        AllMapCache.ClearCache(gc.CharaId);                        
                    }
                }

                if (isCaption)
                {
                    CharacterConfigCache.ClearState(this._site.Config);
                }
            }

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
