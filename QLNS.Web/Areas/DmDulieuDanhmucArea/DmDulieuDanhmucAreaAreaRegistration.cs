using System.Web.Mvc;

namespace QLNS.Web.Areas.DmDulieuDanhmucArea
{
    public class DmDulieuDanhmucAreaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DmDulieuDanhmucArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DmDulieuDanhmucArea_default",
                "DmDulieuDanhmucArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}