using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLNS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLNS.Service.Constant;
using CommonHelper.ObjectExtention;

namespace QLNS.Test.Common
{
    [TestClass]
    public class ConstantExtensionTest
    {
        public ConstantExtensionTest()
        {

        }
        [TestMethod]
        public void TestGetDropDown()
        {
            var listData = ConstantExtension.GetDropdownData<LoaiTaiLieuUploadConstant>(string.Empty);
            var constant=ConstantExtension.GetName<LoaiTaiLieuUploadConstant>("FileHoSoUngVien");
        }
    }
}
