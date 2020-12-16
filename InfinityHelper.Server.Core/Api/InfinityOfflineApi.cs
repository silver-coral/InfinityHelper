using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityOfflineApi : InfinityApiBase
    {
        public InfinityOfflineApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            if (string.IsNullOrEmpty(this._site.Config.CurrentMapId))
            {                
                this._site.Config.CurrentMapId = this._site.InitAllSingleMaps().FirstOrDefault().MapId; 
            }

            this._site.StartOffline();

            CharacterCache.ClearCache(this._site.CurrentCharId);

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
