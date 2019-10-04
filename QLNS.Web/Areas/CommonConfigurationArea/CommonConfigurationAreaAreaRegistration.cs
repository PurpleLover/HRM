using System.Web.Mvc;

namespace QLNS.Web.Areas.CommonConfigurationArea
{
    public class CommonConfigurationAreaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CommonConfigurationArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CommonConfigurationArea_default",
                "CommonConfigurationArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}