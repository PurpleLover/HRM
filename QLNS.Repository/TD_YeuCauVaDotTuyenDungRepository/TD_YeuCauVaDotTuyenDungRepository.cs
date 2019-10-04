using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.TD_YeuCauVaDotTuyenDungRepository
{
    public class TD_YeuCauVaDotTuyenDungRepository : GenericRepository<TD_YeuCauVaDotTuyenDung>, ITD_YeuCauVaDotTuyenDungRepository
    {
        public TD_YeuCauVaDotTuyenDungRepository(DbContext context) : base(context)
        {

        }
    }
}
