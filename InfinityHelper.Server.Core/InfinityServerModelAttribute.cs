using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityServerModelAttribute : Attribute
    {
        public InfinityServerModelAttribute(string name)
        {
            this.TemplateName = name;
        }
        public string TemplateName { get; set; }
    }
}
