using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class LoginModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class LoginResultModel
    {
        public string CharaId { get; set; }
        public string NickName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
