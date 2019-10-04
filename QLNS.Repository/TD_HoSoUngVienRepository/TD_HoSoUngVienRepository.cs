using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.TD_HoSoUngVienRepository
{
    public class TD_HoSoUngVienRepository : GenericRepository<TD_HoSoUngVien>, ITD_HoSoUngVienRepository
    {
        public TD_HoSoUngVienRepository(DbContext context) : base(context)
        {

        }
    }

}
