using QLNS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.AppUserService.Dto
{
    public class AppUserSearchDto : SearchBase
    {
        public string UserNameFilter { get; set; }
        public string FullNameFilter { get; set; }
        public string EmailFilter { get; set; }
        public string AddressFilter { get; set; }

    }
}
