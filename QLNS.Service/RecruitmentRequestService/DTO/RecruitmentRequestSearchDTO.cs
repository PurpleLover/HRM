using QLNS.Model.Entities;
using QLNS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.RecruitmentRequestService.DTO
{
    public class RecruitmentRequestSearchDTO : SearchBase
    {
        public string QueryTitle { get; set; }
        public int? QueryDepartmentId { get; set; }
        public string QueryUntilDateFrom { get; set; }
        public string QueryUntileDateTo { get; set; }
        public string QueryPositions { get; set; }
        public string QueryStatus { get; set; }
        public int? QueryQuantityFrom { get; set; }
        public int? QueryQuantityTo { get; set; }
        public bool QueryTemplate { get; set; }
    }
}
