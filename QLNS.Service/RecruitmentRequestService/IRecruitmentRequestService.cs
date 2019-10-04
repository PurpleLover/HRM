using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.RecruitmentRequestService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.RecruitmentRequestService
{
    public interface IRecruitmentRequestService : IEntityService<RecruitmentRequest>
    {
        RecruitmentRequestDTO GetDataById(long id);
        PageListResultBO<RecruitmentRequestDTO> GetDataByPage(RecruitmentRequestSearchDTO searchParams, int pageIndex = 1, int pageSize = 20);
        List<RecruitmentRequestDTO> GetRecruitmentRequestsNew();
    }
}
