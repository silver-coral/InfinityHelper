using InfinityHelper.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    [InfinityServerModel("config/filter")]
    public class FilterModel : InfinityServerModel
    {
        public FilterModel(InfinityServerSite site) : base(site)
        {
            Initialize();
        }

        public void Initialize()
        {
            this.Character = this._site.CurrentChar;
            this.FilterConfig = this._site.FilterConfig;
        }                       

        public Character Character { get; set; }        
        public ItemFilterConfig FilterConfig { get; set; }
    }
}
