using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.DanhmucRepository
{
    public class NhomDanhmucRepository : GenericRepository<DM_NhomDanhmuc>, IDM_NhomDanhmucRepository
    {
        public NhomDanhmucRepository(DbContext context) : base (context)
        {

        }
    }
}
