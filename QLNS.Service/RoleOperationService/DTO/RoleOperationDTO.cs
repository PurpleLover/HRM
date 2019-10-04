using QLNS.Model.Entities;
using QLNS.Service.ModuleService.DTO;
using QLNS.Service.OperationService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.RoleOperationService.DTO
{
    public class RoleOperationDTO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public IEnumerable<ModuleDTO> GroupModules { get; set; }
    }
}
