using QLNS.Model.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Repository.AppUserRepository
{
    public interface IAppUserRepository:IGenericRepository<AppUser>
    {
        AppUser GetById(long id);

    }
   
}
