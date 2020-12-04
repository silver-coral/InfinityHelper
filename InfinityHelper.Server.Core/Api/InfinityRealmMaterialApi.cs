using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityRealmMaterialApi : InfinityApiBase
    {
        public InfinityRealmMaterialApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {            
            var material = this._site.RealmUpMaterial();

            string templatePath = RazorHtmlHelper.ResolveTemplatePath("/Character/RealmMaterialRender.cshtml");
            var builder = new CodeBuilder<RealmUpMaterial>(templatePath, material);
            string html = builder.Generate();

            Response.Write(html);
        }
    }
}
