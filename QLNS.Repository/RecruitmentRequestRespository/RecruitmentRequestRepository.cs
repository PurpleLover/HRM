using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.RecruitmentRequestRespository
{
    public class RecruitmentRequestRepository : GenericRepository<RecruitmentRequest>, IRecruitmentRequestRepository
    {
        public RecruitmentRequestRepository(DbContext context) : base(context)
        {
        }
    }
}
