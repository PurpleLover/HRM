using System.Web.Mvc;

namespace QLNS.Web.Areas.RecruitmentSkillDetailArea
{
    public class RecruitmentSkillDetailAreaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RecruitmentSkillDetailArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RecruitmentSkillDetailArea_default",
                "RecruitmentSkillDetailArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}