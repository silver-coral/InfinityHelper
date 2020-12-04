using InfinityHelper.Server.Core;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 10000;

                string url = System.Configuration.ConfigurationManager.AppSettings["ProxyServer"];

                using (WebApp.Start<Startup>(url))
                {
                    Logger.Info("【Init】StartProxyServer--{0}", url);
                    Console.Read();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            Console.Read();
        }
    }
}
