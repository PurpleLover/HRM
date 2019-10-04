using Autofac;
using Autofac.Integration.Mvc;
using log4net;
using QLNS.Modules;
using QLNS.Web.App_Start;
using QLNS.Web.Filters;
using QLNS.Web.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace QLNS.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
       
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Autofac Configuration
            var builder = new Autofac.ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();
            builder.RegisterModule(new RepositoryModule());

            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EFModule());
            builder.RegisterModule(new LoggingModule());
            builder.RegisterModule(new AutoMapperModule());
            builder.RegisterModule(new RedisModule());
            
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
         
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            ILog log = LogManager.GetLogger("RollingLogFileAppender");
         
            var exception = Server.GetLastError();
            log.Error("Lỗi hệ thống", exception);
            if (exception is HttpUnhandledException)
            {
                log.Error("Lỗi hệ thống", exception);
            }
           
        }

    }
}
