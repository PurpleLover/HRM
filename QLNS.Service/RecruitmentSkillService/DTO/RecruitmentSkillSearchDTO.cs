using QLNS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.RecruitmentSkillService.DTO
{
    public class RecruitmentSkillSearchDTO : SearchBase
    {
        public string QueryTitle { get; set; }
    }
}
