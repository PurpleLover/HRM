using QLNS.Model.Entities;
using QLNS.Service.OperationService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.ModuleService.DTO
{
    public class ModuleDTO : Module
    {
        public int OperationQuantity { set; get; }
        public IEnumerable<OperationDTO> GroupOperations { get; set; }
    }
}
