using QLNS.Model.Entities;
using QLNS.Service.RoleOperationService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.RoleOperationService
{
    public interface IRoleOperationService : IEntityService<RoleOperation>
    {
        RoleOperationDTO GetConfigureOperation(int roleId);
    }
}
