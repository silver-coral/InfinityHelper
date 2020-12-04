using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityEquipUpgradeMaterialApi : InfinityApiBase
    {
        public InfinityEquipUpgradeMaterialApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            string eid = GetQuery("eid");
            var material = this._site.EquipUpMaterial(eid);

            string templatePath = RazorHtmlHelper.ResolveTemplatePath("/Equip/MaterialRender.cshtml");
            var builder = new CodeBuilder<EquipUpMaterial>(templatePath, material);
            string html = builder.Generate();

            Response.Write(html);
        }
    }
}
