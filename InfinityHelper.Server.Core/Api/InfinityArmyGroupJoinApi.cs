using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityArmyGroupJoinApi : InfinityApiBase
    {
        public InfinityArmyGroupJoinApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            string aid = GetQuery("aid");
           
            this._site.ArmyGroupJoin(aid);
            
            CharacterConfigCache.ClearState(this._site.Config);

            AllMapCache.ClearCache(this._site.CurrentCharId);

            CharacterArmyGroupCache.ClearCache(this._site.CurrentCharId);
            var army = this._site.InitArmyGroup();
            if (army != null)
            {
                foreach (var c in army.CharaInfoVoList)
                {
                    var gc = CharacterCache.GetCharByNo(c.CharaNo);
                    if (gc != null && gc.CharaId != this._site.CurrentCharId)
                    {
                        CharacterArmyGroupCache.ClearCache(gc.CharaId);
                    }
                }
            }

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
