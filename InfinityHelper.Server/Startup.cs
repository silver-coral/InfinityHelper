using System;
using System.Threading.Tasks;
using InfinityHelper.Server.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(InfinityHelper.Server.Startup))]

namespace InfinityHelper.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            BattleScheduler.OnBattleComplete = bs_onBattleComplete;

            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = false; //禁用详细错误信息
            hubConfiguration.EnableJavaScriptProxies = false; //禁用自动的代理类           
            app.MapSignalR("/signalr", hubConfiguration);

            app.Use(typeof(InfinityServerOwinMiddleware));
        }

        private void bs_onBattleComplete(string charId, BattleResult bt)
        {
            try
            {
                UserManagerHub.GetClients(charId).battleLog(JsonUtil.Serialize(bt)); 
            }
            catch { }
        }
    }
}
