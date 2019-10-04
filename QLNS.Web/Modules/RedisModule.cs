using Autofac;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using CommonHelper.String;

namespace QLNS.Web.Modules
{
    public class RedisModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
            .Register(cx => ConnectionMultiplexer.Connect(string.Format("{0}:{1}", WebConfigurationManager.AppSettings["RedisHost"], WebConfigurationManager.AppSettings["RedisPort"])))
            .As<IConnectionMultiplexer>()
            .SingleInstance();
            builder.Register(c => c.Resolve<IConnectionMultiplexer>()
                 .GetDatabase(WebConfigurationManager.AppSettings["RedisDatabase"].ToIntOrZero()))
                 .As<IDatabase>()
                 .SingleInstance();
        }
    }
}