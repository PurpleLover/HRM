using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.CommonConfigurationRepository
{
    public class CommonConfigurationRepository : GenericRepository<CommonConfiguration>, ICommonConfigurationRepository
    {
        public CommonConfigurationRepository(DbContext context) : base(context) { }
    }
}
