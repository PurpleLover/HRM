using CommonHelper.Validation;
using QLNS.Service.Common;
using QLNS.Service.RecruitmentSkillService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Web.Areas.RecruitmentSkillArea.Models
{
    public class RecruitmentSkillViewModel
    {
        public class RecruitmentSkillIndexViewModel
        {
            public PageListResultBO<RecruitmentSkillDTO> GroupData { get; set; }
        }

        public class RecruitmentKillEditViewModel
        {
            public long Id { get; set; }
            [RequiredExtend]
            [StringLengthExtends(250)]
            [HTMLInjection]
            [SpecialCharacter]
            public string Title { get; set; }

            /// <summary>
            /// danh sách kỹ năng
            /// </summary>
            [RequiredExtend]
            public long[] Skills { get; set; }

            public List<SelectListItem> GroupSkills { get; set; }
        }
    }
}