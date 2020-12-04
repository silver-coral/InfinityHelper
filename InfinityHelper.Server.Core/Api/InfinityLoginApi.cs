using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityLoginApi : InfinityApiBase
    {
        public InfinityLoginApi(InfinityServerSite site) : base(site) { }

        public override void Execute()
        {
            LoginModel data = GetPostData<LoginModel>();            

            try
            {
                this._site.Login(data);
                Response.Write(JsonUtil.Serialize(new { IsOk = true }));
            }
            catch (Exception ex)
            {
                Response.Write(JsonUtil.Serialize(new { IsOk = false, Msg = ex.Message }));
            }
        }
    }
}
