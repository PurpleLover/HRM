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
namespace QLNS.Service.TD_HoSoUngVienService.DTO
{

    public class HoSoUngVienDTO : TD_HoSoUngVien
    {
        public string TenTrangThai
        {
            get
            {
                return ConstantExtension.GetName<HoSoUngVienTrangThaiConst>(TrangThai);
            }
            set
            {

            }
        }

    }

}
