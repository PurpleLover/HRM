using AutoMapper;
using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration.Conventions;
using QLNS.Service.Common;
using QLNS.Service.Constant;
using System.Web;

namespace QLNS.Service.TD_QuanLyMauTestService.DTO
{

    public class QuanLyMauTestDTO : TD_QuanLyMauTest
    {

    }
    public class SaveFileModel
    {
        public HttpPostedFileBase File { get; set; }
        public string Name { get; set; }
        public string ExtensionList { get; set; }
        public int? MaxSize { get; set; }
        public string Folder { get; set; }
        public string Path { get; set; }
    }
}
