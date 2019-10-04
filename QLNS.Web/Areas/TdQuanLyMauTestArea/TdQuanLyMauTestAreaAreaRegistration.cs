using System.Web.Mvc;

namespace QLNS.Web.Areas.TdQuanLyMauTestArea
{
    public class TdQuanLyMauTestAreaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TdQuanLyMauTestArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TdQuanLyMauTestArea_default",
                "TdQuanLyMauTestArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}