using QLNS.Service.Common;
using QLNS.Service.DM_DulieuDanhmucService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNS.Web.Areas.DmDulieuDanhmucArea.Models
{
    public class IndexVM
    {
        public PageListResultBO<DM_DulieuDanhmucDTO> Data { get; set; }
        public long? GroupId { get; set; }
    }
}