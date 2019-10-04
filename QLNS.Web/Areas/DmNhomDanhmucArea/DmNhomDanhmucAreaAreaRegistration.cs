using System.Web.Mvc;

namespace QLNS.Web.Areas.DmNhomDanhmucArea
{
    public class DmNhomDanhmucAreaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DmNhomDanhmucArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DmNhomDanhmucArea_default",
                "DmNhomDanhmucArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}