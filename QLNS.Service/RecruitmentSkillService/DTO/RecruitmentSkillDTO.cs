using QLNS.Model.Entities;
using QLNS.Service.RecruitmentSkillDetailService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.RecruitmentSkillService.DTO
{
    public class RecruitmentSkillDTO : RecruitmentSkill
    {
        public int NumberOfSkills { get; set; }

        public List<RecruitmentSkillDetailDTO> GroupSkillDetails { get; set; }
    }
}
