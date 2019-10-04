using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.OperationRepository
{
    public class OperationRepository : GenericRepository<Operation>, IOperationRepository
    {
        public OperationRepository(DbContext context) : base(context)
        {
        }
    }
}
