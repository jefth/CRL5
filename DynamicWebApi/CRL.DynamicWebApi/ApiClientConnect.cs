﻿using ImpromptuInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRL.DynamicWebApi
{
    public class ApiClientConnect
    {
        string host;
        public ApiClientConnect(string _host)
        {
            host = _host;
        }
        string user, token;
        public void SetToken(string _user, string _token)
        {
            user = _user;
            token = _token;
        }
        Dictionary<string, object> _services = new Dictionary<string, object>();
        public T GetClient<T>() where T : class
        {
            var serviceName = typeof(T).Name;
            var key = string.Format("{0}_{1}", host, serviceName);
            var a = _services.TryGetValue(key, out object instance);
            if (a)
            {
                return instance as T;
            }
            var client = new ApiClient
            {
                Host = host,
                ServiceType = typeof(T),
                ServiceName = serviceName,
                Token = string.Format("{0}@{1}", user, token)
            };
            //创建代理
            instance = client.ActLike<T>();
            _services[key] = instance;
            return instance as T;
        }
    }
}