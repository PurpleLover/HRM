using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.TD_DotTuyenDungRepository
{
    public class TD_DotTuyenDungRepository : GenericRepository<TD_DotTuyenDung>, ITD_DotTuyenDungRepository
    {
        public TD_DotTuyenDungRepository(DbContext context) : base(context)
        {
        }
    }
}
