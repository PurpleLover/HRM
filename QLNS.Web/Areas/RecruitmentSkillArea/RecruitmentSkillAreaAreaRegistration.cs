using System.Web.Mvc;

namespace QLNS.Web.Areas.RecruitmentSkillArea
{
    public class RecruitmentSkillAreaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RecruitmentSkillArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RecruitmentSkillArea_default",
                "RecruitmentSkillArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}