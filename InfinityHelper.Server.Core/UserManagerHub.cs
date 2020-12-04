using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class UserManagerHub : Hub
    {
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, UserConnection>> _onLineChars = new ConcurrentDictionary<string, ConcurrentDictionary<string, UserConnection>>();
        public static ConcurrentDictionary<string, ConcurrentDictionary<string, UserConnection>> OnLineChars { get { return _onLineChars; } }
        /// <summary>
        /// 是否在线（存在连接）
        /// </summary>
        /// <param name="charId"></param>
        /// <returns></returns>
        public static bool IsOnLine(string charId)
        {
            return _onLineChars.ContainsKey(charId);
        }        

        /// <summary>
        /// 移除某个User的所有连接
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool RemoveUser(string charId)
        {
            ConcurrentDictionary<string, UserConnection> connList = null;
            return _onLineChars.TryRemove(charId, out connList);
        }

        #region 获取指定连接

        private static IHubContext Instance
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<UserManagerHub>();
            }
        }

        public static dynamic GetAllClients()
        {
            return Instance.Clients.All;
        }

        private static List<string> GetClientIds(string charId)
        {
            ConcurrentDictionary<string, UserConnection> connList = null;
            if (!_onLineChars.TryGetValue(charId, out connList))
            {
                return new List<string>();
            }
            return connList.Keys.ToList();
        }

        public static dynamic GetClients(string charId)
        {
            List<string> clientIdList = GetClientIds(charId);
            return Instance.Clients.Clients(clientIdList);
        }

        public static dynamic GetClient(string connectionId)
        {
            return Instance.Clients.Client(connectionId);
        }

        #endregion

        /// <summary>
        /// 连接用户信息
        /// </summary>
        /// <returns></returns>
        private string GetCurrentCharId()
        {
            if (this.Context.RequestCookies.ContainsKey("charaId"))
            {
                return this.Context.RequestCookies["charaId"].Value;
            }            
            return null;
        }

        /// <summary>
        /// 新连接接入
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnConnected()
        {
            Task result = null;
            try
            {
                //添加Connection
                var referers = this.Context.Request.Headers.GetValues("Referer");
                if (referers == null || referers.Count() == 0)
                {
                    throw new ApplicationException("referer == null");
                }

                UserConnection conn = new UserConnection()
                {
                    ConnectionId = this.Context.ConnectionId,
                    CharId = GetCurrentCharId(),
                    PageUrl = referers.First(),
                    ConnectTime = DateTime.Now,
                };
                var userAgents = this.Context.Request.Headers.GetValues("User-Agent");
                if (userAgents != null && userAgents.Count() > 0)
                {
                    conn.UserAgent = userAgents.First();
                }

                ConcurrentDictionary<string, UserConnection> connList = null;
                if (_onLineChars.TryGetValue(conn.CharId, out connList))
                {
                    connList.TryAdd(conn.ConnectionId, conn);
                }
                else
                {
                    connList = new ConcurrentDictionary<string, UserConnection>();
                    connList.TryAdd(conn.ConnectionId, conn);
                    _onLineChars.TryAdd(conn.CharId, connList);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                result = base.OnConnected();
            }
            return result;
        }

        /// <summary>
        /// 连接断开
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            Task result = null;

            try
            {
                string charId = GetCurrentCharId();

                ConcurrentDictionary<string, UserConnection> connList = null;
                if (_onLineChars.TryGetValue(charId, out connList))
                {
                    if (connList.ContainsKey(this.Context.ConnectionId))
                    {
                        UserConnection conn = null;
                        connList.TryRemove(this.Context.ConnectionId, out conn);
                    }

                    if (connList.Count == 0)
                    {
                        _onLineChars.TryRemove(charId, out connList);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                result = base.OnDisconnected(stopCalled);
            }
            return result;
        }

        /// <summary>
        /// 重新连接
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
    }

    /// <summary>
    /// 代表一个连接
    /// </summary>
    public class UserConnection
    {
        /// <summary>
        /// 用户信息（Cookie）
        /// </summary>
        public string CharId { get; set; }

        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectTime { get; set; }

        /// <summary>
        /// 连接key
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// 页面URL
        /// </summary>
        public string PageUrl { get; set; }

        public string UserAgent { get; set; }

        public bool IsMobile
        {
            get
            {
                bool result = false;
                if (!string.IsNullOrEmpty(this.UserAgent))
                {
                    string ua = this.UserAgent.ToLower();
                    result = ua.Contains("iphone") || ua.Contains("ipad") || ua.Contains("android") || ua.Contains("macintosh");
                }
                return result;
            }
        }
    }
}
