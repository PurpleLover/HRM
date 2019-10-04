using CommonHelper.Validation;
using QLNS.Service.Common;
using QLNS.Service.RecruitmentRequestService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Web.Areas.RecruitmentRequestArea.Models
{
    public class TemplateRecruitmentRequestViewModel
    {
        public class TemplateRecruitmentRequestIndexViewModel
        {
            public PageListResultBO<RecruitmentRequestDTO> GroupData { get; set; }
        }

        public class TemplateRecruitmentRequestEditViewModel
        {
            public long Id { get; set; }

            [RequiredExtend]
            [StringLengthExtends(250)]
            public string Title { get; set; }

            [RequiredExtend]
            public long[] SkillIds { get; set; }

            public IEnumerable<SelectListItem> GroupSkills { get; set; }
        }
    }
}