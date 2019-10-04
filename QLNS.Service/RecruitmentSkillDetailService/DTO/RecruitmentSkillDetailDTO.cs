using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.RecruitmentSkillDetailService.DTO
{
    public class RecruitmentSkillDetailDTO : RecruitmentSkillDetail
    {
        public string DataTypeName { get; set; }
        public string DataTypeValue { get; set; }
        public IEnumerable<DM_DulieuDanhmuc> GroupCategoryData { get; set; }
    }
}
