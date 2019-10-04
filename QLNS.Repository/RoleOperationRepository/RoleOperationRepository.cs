using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.RoleOperationRepository
{
    public class RoleOperationRepository : GenericRepository<RoleOperation>, IRoleOperationRepository
    {
        public RoleOperationRepository(DbContext context) : base(context)
        {
        }
    }
}
