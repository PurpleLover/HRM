using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.TD_CauHinhSangLocUngVienService.DTO
{
    public class CauHinhSangLocUngVienDTO : TD_CauHinhSangLocUngVien
    {
        public string FileName { get; set; }
        public string FileDirectory { get; set; }
    }
}
