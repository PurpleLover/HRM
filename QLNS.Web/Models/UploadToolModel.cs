using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNS.Web.Models
{
    public class UploadToolModel
    {
        public List<TaiLieuDinhKem> LstTaiLieu { get; set; }
        public bool IsModify { get; set; }
        public string LoaiTaiLieu { get; set; }
    }
}