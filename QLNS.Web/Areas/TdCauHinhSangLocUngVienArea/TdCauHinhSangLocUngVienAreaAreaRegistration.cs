using System.Web.Mvc;

namespace QLNS.Web.Areas.TdCauHinhSangLocUngVienArea
{
    public class TdCauHinhSangLocUngVienAreaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TdCauHinhSangLocUngVienArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TdCauHinhSangLocUngVienArea_default",
                "TdCauHinhSangLocUngVienArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}