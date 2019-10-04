using QLNS.Model.Entities;
using QLNS.Repository.ConfigRecruitmentRequestRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.RecruitmentRequestRepository
{
    public class ConfigRecruitmentRequestRepository : GenericRepository<ConfigRecruitmentRequest>, IConfigRecruitmentRequestRepository
    {
        public ConfigRecruitmentRequestRepository(DbContext context) : base(context)
        {
        }
    }
}
