using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.UserRoleService
{
    public interface IUserRoleService:IEntityService<UserRole>
    {
        List<UserRole> GetRoleOfUser(long userId);
        bool SaveRole(List<int> listRoleId, long UserId);
    }
}
