using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLNS.Service.TD_CauHinhSangLocUngVienService.DTO;

namespace QLNS.Web.Areas.TdCauHinhSangLocUngVienArea.Models
{
    public class IndexVM
    {
        public List<CauHinhSangLocUngVienDTO> Data { get; set; }
        public long DotTuyenDungId { get; set; }
    }
}