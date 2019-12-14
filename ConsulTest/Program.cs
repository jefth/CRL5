﻿using CRL.Core.Remoting;
using CRL.DynamicWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsulTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //1.启动consol
            //2.启动CRL.Ocelot,配置见configuration.json
            var server = new ServerCreater().CreatetApi();
            server.Register<ITestService, TestService>();
            var listener = new ServerListener();
            listener.Start("http://localhost:809/");//启用apiService1
            //注册服务
            var consulClient = new CRL.Core.ConsulClient.Consul("http://localhost:8500");
            consulClient.UseOcelotGatewayDiscover("http://localhost:3400"); //使用网关服务发现
            var info = new CRL.Core.ConsulClient.ServiceRegistrationInfo
            {
                Address = "localhost",
                Name = "apiService1",
                ID = "apiService1",
                Port = 809,
                Tags = new[] { "v1" }
            };
            consulClient.DeregisterService(info.ID);
            var a = consulClient.RegisterService(info);//注册apiService1
            var clientConnect = new ApiClientConnect("");
            clientConnect.UseConsulApiGatewayDiscover("http://localhost:3400", "apiService1");//服务发现

            var clientConnect2 = new ApiClientConnect("");
            clientConnect2.UseConsulApiGateway("http://localhost:3400");//直接使用网关

        label1:

            var service1 = clientConnect.GetClient<ITestService>();
            var msg = service1.Login();
            Console.WriteLine("服务发现调用成功:" + msg);

            var service2 = clientConnect2.GetClient<ITestService>("serviceTest");
            msg = service2.Login();
            Console.WriteLine("服务网关调用成功:" + msg);

            Console.ReadLine();

            goto label1;
        }
    }
}