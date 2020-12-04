using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    /// <summary>
    /// 统一返回对象
    /// </summary>
    public class Resp
    {
        /// <summary>
        /// 判断执行结果正确与否
        /// </summary>
        public string Status
        {
            get; set;
        }

        /// <summary>
        /// 执行失败时的错误消息
        /// </summary>
        public string Msg { get; set; }
        public bool IsOk { get { return Status == "200"; } }
    }

    /// <summary>
    /// 带数据的统一返回对象
    /// </summary>
    /// <typeparam name="T">需要返回的数据类型</typeparam>    
    public class Resp<T> : Resp
    {
        /// <summary>
        /// 需要返回的数据
        /// </summary>
        public T Data
        {
            get;
            set;
        }
    }
}
