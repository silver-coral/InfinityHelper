using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityArmyGroupRemoveApi : InfinityApiBase
    {
        public InfinityArmyGroupRemoveApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            string no = GetQuery<string>("no");

            var army = this._site.InitArmyGroup();
            if (army != null)
            {
                this._site.ArmyGroupRemoveChar(no);

                foreach (var c in army.CharaInfoVoList)
                {
                    var gc = CharacterCache.GetCharByNo(c.CharaNo);
                    if(gc != null)
                    {
                        CharacterArmyGroupCache.ClearCache(gc.CharaId);
                    }
                }

                AllMapCache.ClearCache(this._site.CurrentCharId);
            }

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
