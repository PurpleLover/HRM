using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.RecruitmentSkillDetailService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.RecruitmentSkillDetailService
{
    public interface IRecruitmentSkillDetailService : IEntityService<RecruitmentSkillDetail>
    {
        PageListResultBO<RecruitmentSkillDetailDTO> GetDataByPage(RecruitmentSkillDetailSearchDTO searchParams, int pageIndex = 1, int pageSize = 20);
    }
}
