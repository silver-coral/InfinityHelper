using InfinityHelper.Server.Core;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server
{    
    public class InfinityServerOwinMiddleware : OwinMiddleware
    {
        public InfinityServerOwinMiddleware(OwinMiddleware next) : base(next) { }

        public override async Task Invoke(IOwinContext context)
        {
            InfinityServerHandler handler = new InfinityServerHandler(context);
            handler.ProcessRequest();

            await Task.FromResult(0);
        }
    }
}
