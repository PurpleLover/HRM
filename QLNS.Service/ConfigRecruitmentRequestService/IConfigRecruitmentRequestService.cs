using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.ConfigRecruitmentRequestService
{
    public interface IConfigRecruitmentRequestService : IEntityService<ConfigRecruitmentRequest>
    {
        IEnumerable<ConfigRecruitmentRequest> GetConfigByRequest(RecruitmentRequest request);
    }
}
