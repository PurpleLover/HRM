using QLNS.Model.Entities;
using QLNS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.OperationService.DTO
{
    public class OperationSearchDTO: SearchBase
    {
        public int QueryModuleId { get; set; }
        public string QueryName { get; set; }
        public bool? QueryIsShow { get; set; }
    }
}
