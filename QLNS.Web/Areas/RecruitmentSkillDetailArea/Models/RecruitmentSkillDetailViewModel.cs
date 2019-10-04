using CommonHelper.Validation;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.RecruitmentSkillDetailService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Web.Areas.RecruitmentSkillDetailArea.Models
{
    public class RecruitmentSkillDetailViewModel
    {
        public class RecruitmentSkillDetailIndexViewModel
        {
            public List<SelectListItem> GroupDataType { get; set; }
            public PageListResultBO<RecruitmentSkillDetailDTO> GroupData { get; set; }
        }

        public class RecruitmentSkillDetailEditViewModel
        {
            public RecruitmentSkillDetail EditEntity { get; set; }
            public List<SelectListItem> GroupDataType { get; set; }
        }
    }
}