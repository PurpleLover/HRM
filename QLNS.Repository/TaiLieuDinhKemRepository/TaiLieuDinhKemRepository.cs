using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.TaiLieuDinhKemRepository
{
    public class TaiLieuDinhKemRepository : GenericRepository<TaiLieuDinhKem>, ITaiLieuDinhKemRepository
    {
        public TaiLieuDinhKemRepository(DbContext context) : base(context)
        {

        }
    }
}
