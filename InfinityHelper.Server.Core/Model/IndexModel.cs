using InfinityHelper.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    [InfinityServerModel("home/index")]
    public class IndexModel : InfinityServerModel
    {
        public IndexModel(InfinityServerSite site) : base(site)
        {
            Initialize();
        }

        public void Initialize()
        {
            Character result = this._site.CurrentChar;
            this.CharacterList = new List<Character>() { result };
        }

        public List<Character> CharacterList { get; set; }
    }
}
