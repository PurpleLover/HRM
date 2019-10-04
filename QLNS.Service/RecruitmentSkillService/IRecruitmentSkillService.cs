using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.RecruitmentSkillService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.RecruitmentSkillService
{
    public interface IRecruitmentSkillService : IEntityService<RecruitmentSkill>
    {
        PageListResultBO<RecruitmentSkillDTO> GetDataByPage(RecruitmentSkillSearchDTO searchParams, int pageIndex = 1, int pageSize = 20);

        List<RecruitmentSkillDTO> GetSkillGroupByRequestId(long requestId);

        List<RecruitmentSkillDTO> GetConfigDataOfRequest(long requestId);

        RecruitmentSkillDTO GetDataById(long id);
    }
}
