using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.TD_CauHinhSangLocUngVienRepository
{
    public class TD_CauHinhSangLocUngVienRepository : GenericRepository<TD_CauHinhSangLocUngVien>, ITD_CauHinhSangLocUngVienRepository
    {
        public TD_CauHinhSangLocUngVienRepository(DbContext context) : base(context)
        {

        }
    }
}
