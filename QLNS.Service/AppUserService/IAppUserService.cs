using QLNS.Model.IdentityEntities;
using QLNS.Service.AppUserService.Dto;
using QLNS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.AppUserService
{
    public interface IAppUserService:IEntityService<AppUser>
    {
        PageListResultBO<AppUser> GetDaTaByPage(AppUserSearchDto searchModel, int pageIndex = 1, int pageSize = 20);
        AppUser GetById(long id);
        bool CheckExistUserName(string userName, long? id=null);
        bool CheckExistEmail(string email, long? id=null);
    }
}
