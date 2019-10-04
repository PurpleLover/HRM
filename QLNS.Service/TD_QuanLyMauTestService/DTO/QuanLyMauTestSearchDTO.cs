using QLNS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.TD_QuanLyMauTestService.DTO
{
    public class QuanLyMauTestSearchDTO : SearchBase
    {
        public string QueryName { get; set; }
        public List<string> QueryCategoryList { get; set; }
    }
}
