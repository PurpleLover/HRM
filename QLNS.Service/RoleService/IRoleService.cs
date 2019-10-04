using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.RoleService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.RoleService
{
    public interface IRoleService : IEntityService<Role>
    {
        PageListResultBO<RoleDTO> GetDataByPage(RoleSearchDTO searchParams, int pageIndex = 1, int pageSize = 20);
    }
}
