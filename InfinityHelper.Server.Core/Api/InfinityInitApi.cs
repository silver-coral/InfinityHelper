using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityInitApi : InfinityApiBase
    {
        public InfinityInitApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            var allMaps = this._site.InitAllMaps();
            var dynamicList = CharacterDynamicCache.LoadAllDynamics();
            foreach (var cd in dynamicList)
            {
                foreach (var mi in cd.ItemList)
                {
                    var map = allMaps.FirstOrDefault(p => p.MapId == mi.Key);
                    if (map != null)
                    {
                        foreach (var item in mi.Value)
                        {
                            if (!map.ItemsVoList.ContainsKey(item.Key))
                            {
                                map.ItemsVoList[item.Key] = item.Value;
                            }
                        }
                    }
                }
            }

            var cfgList = CharacterConfigCache.LoadAllConfigs();

            foreach(var cfg in cfgList)
            {
                InfinityServerSite site = new InfinityServerSite(this._site.Uri, this.Request, this.Response);
                site.CurrentCharId = cfg.CharId; 
                site.Config = CharacterConfigCache.TryGetValue(cfg.CharId, CharacterConfigCache.LoadConfig);
                site.Dynamic = CharacterDynamicCache.TryGetValue(cfg.CharId, CharacterDynamicCache.LoadDynamic);
                site.FilterConfig = ItemFilterCache.TryGetValue(cfg.CharId, ItemFilterCache.LoadConfig);
                try
                {
                    site.InitChar();

                    var group = site.InitArmyGroup();

                    if (cfg.IsGuaji)
                    {
                        if (group == null)
                        {
                            BattleScheduler.AddChar(site);
                        }
                        else
                        {
                            site.Config.IsGuaji = false;
                            CharacterConfigCache.SaveConfig(site.Config);
                        }
                    }
                    if (cfg.IsDungeonGuaji)
                    {
                        if (group != null)
                        {
                            if (site.CheckIsGroupCaption())
                            {
                                BattleScheduler.AddChar(site, true);
                            }
                            else
                            {
                                site.Config.IsDungeonGuaji = false;
                                CharacterConfigCache.SaveConfig(site.Config);
                            }
                        }
                    }
                }
                catch { }
            }

            Response.WriteAsync(JsonUtil.Serialize(new { }));
        }
    }
}
