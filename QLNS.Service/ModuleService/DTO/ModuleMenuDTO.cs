using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.ModuleService.DTO
{
    public class ModuleMenuDTO:Module
    {
        public List<Operation> ListOperation { get; set; }
    }
}
