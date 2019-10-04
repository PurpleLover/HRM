using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.ModuleService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.ModuleService
{
    public interface IModuleService : IEntityService<Module>
    {
        PageListResultBO<ModuleDTO> GetDataByPage(ModuleSearchDTO searchParams, int pageIndex = 1, int pageSize = 20);
        bool CheckExistCode(string code, long? id = null);
    }
}
