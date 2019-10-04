using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.DanhmucRepository
{
   public class DM_DulieuDanhmucRepository : GenericRepository<DM_DulieuDanhmuc>, IDM_DulieuDanhmucRepository
    {
        public DM_DulieuDanhmucRepository(DbContext context) : base(context)
        {

        }
    }
}
