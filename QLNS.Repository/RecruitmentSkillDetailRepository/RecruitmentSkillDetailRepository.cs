using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.RecruitmentSkillDetailRepository
{
    public class RecruitmentSkillDetailRepository : GenericRepository<RecruitmentSkillDetail>, IRecruitmentSkillDetailRepository
    {
        public RecruitmentSkillDetailRepository(DbContext context) : base(context)
        {
        }
    }
}
