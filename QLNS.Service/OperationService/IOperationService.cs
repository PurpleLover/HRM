using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.ModuleService.DTO;
using QLNS.Service.OperationService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.OperationService
{
    public interface IOperationService : IEntityService<Operation>
    {
        PageListResultBO<OperationDTO> GetDataByPage(OperationSearchDTO searchParams, int pageIndex = 1, int pageSize = 20);
        List<ModuleMenuDTO> GetListOperationOfUser(long userId);
        bool CheckCode(string code, long? id = null);
    }
}
