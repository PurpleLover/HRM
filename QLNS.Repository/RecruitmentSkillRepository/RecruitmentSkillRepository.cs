using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.RecruitmentSkillRepository
{
    public class RecruitmentSkillRepository : GenericRepository<RecruitmentSkill>, IRecruitmentSkillRepository
    {
        public RecruitmentSkillRepository(DbContext context) : base(context)
        {
        }
    }
}
