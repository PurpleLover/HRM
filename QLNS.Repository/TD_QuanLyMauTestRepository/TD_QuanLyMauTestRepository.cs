using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.TD_QuanLyMauTestRepository
{
    public class TD_QuanLyMauTestRepository : GenericRepository<TD_QuanLyMauTest>, ITD_QuanLyMauTestRepository
    {
        public TD_QuanLyMauTestRepository(DbContext context) : base(context)
        {

        }
    }
}
