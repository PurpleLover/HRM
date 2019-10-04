using System.Web.Mvc;

namespace QLNS.Web.Areas.TDHoSoUngVienArea
{
    public class TDHoSoUngVienAreaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TDHoSoUngVienArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TDHoSoUngVienArea_default",
                "TDHoSoUngVienArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}