using System.Web.Mvc;

namespace QLNS.Web.Areas.DotTuyenDungArea
{
    public class DotTuyenDungAreaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DotTuyenDungArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DotTuyenDungArea_default",
                "DotTuyenDungArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}