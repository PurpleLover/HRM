using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.RecruitmentRequestService.DTO
{
    public class RecruitmentRequestDTO : RecruitmentRequest
    {
        /// <summary>
        /// tên vị trí
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// tên phòng ban
        /// </summary>
        public string DepartmentName { get; set; }

        public IEnumerable<RecruitmentSkill> DataGroupSkills { get; set; }
    }
}
