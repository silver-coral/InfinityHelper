using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityClearMapItemApi : InfinityApiBase
    {
        public InfinityClearMapItemApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {            
            var dynamicList = CharacterDynamicCache.LoadAllDynamics();
            foreach (var cd in dynamicList)
            {
                cd.ItemList = new Dictionary<string, Dictionary<string, MapItem>>();

                CharacterDynamicCache.SaveDynamic(cd);
            }            

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
