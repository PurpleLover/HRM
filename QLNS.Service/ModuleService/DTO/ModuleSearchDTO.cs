using QLNS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.ModuleService.DTO
{
    public class ModuleSearchDTO : SearchBase
    {
        public string QueryName { get; set; }
        public bool? QueryIsShow { get; set; }
        public string QueryIcon { get; set; }
        public string QueryClassCss { get; set; }
        public string QueryStyleCss { get; set; }
        public string QueryCode { get; set; }
    }
}
