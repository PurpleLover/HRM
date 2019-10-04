using QLNS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.CommonConfigurationService.DTO
{
    public class CommonConfigurationSearchDTO : SearchBase
    {
        public string QueryName { get; set; }
        public string QueryCode { get; set; }
    }
}
