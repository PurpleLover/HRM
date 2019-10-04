using QLNS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.RecruitmentSkillDetailService.DTO
{
    public class RecruitmentSkillDetailSearchDTO : SearchBase
    {
        public string QueryName { get; set; }
        public int? QueryType { get; set; }
    }
}
