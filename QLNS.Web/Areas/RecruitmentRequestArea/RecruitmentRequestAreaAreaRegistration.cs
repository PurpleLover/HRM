using System.Web.Mvc;

namespace QLNS.Web.Areas.RecruitmentRequestArea
{
    public class RecruitmentRequestAreaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RecruitmentRequestArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RecruitmentRequestArea_default",
                "RecruitmentRequestArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}