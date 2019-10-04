using QLNS.Model.Common;
using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonHelper.ObjectExtention;

namespace QLNS.Model.Seed
{
    public class InitDanhMucSeed
    {
        public static void Init(QLNSContext context)
        {
            
            var DanTocGroup= context.DM_NhomDanhmuc.Where(x => x.GroupCode == DanhMucConstantBase.DanToc).FirstOrDefault();
            if (DanTocGroup==null)
            {
                context.DM_NhomDanhmuc.Add(new DM_NhomDanhmuc()
                {
                    GroupCode = DanhMucConstantBase.DanToc,
                    GroupName = DanhMucConstantBase.DanToc
                });
                context.SaveChanges();
            }

        }
    }
}
