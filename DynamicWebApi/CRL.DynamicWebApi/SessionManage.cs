﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRL.DynamicWebApi
{
    public class SessionManage
    {
        static ConcurrentDictionary<string, string> sessions = new ConcurrentDictionary<string, string>();
        /// <summary>
        /// 登录后返回新的TOKEN
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        public static void SaveSession(string user, string token)
        {
            if (!sessions.TryGetValue(user, out string token2))
            {
                sessions.TryAdd(user, token);
            }
            else
            {
                sessions[user] = token;
            }
            Core.CallContext.SetData("newToken", string.Format("{0}@{1}", user, token));
        }
        /// <summary>
        /// 返回登录名
        /// </summary>
        /// <returns></returns>
        public static string GetSession()
        {
            return Core.CallContext.GetData<string>("currentUser");
        }
        internal static void CheckSession(string user,string token)
        {
            var exists = sessions.TryGetValue(user, out string v);
            if (!exists)
            {
                throw new Exception("API未登录");
            }
            if (token != v)
            {
                throw new Exception("token验证失败");
            }
        }
    }
}
