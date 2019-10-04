using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.DepartmentService.DTO
{
    public class DepartmentDTO : Department
    {
        public List<DepartmentDTO> Child { get; set; }
    }

    public class DepartmentCM
    {
        public int id { get; set; }
        public List<DepartmentCM> children { get; set; }
    }
}
