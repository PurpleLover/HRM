using QLNS.Model.Entities;
using QLNS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLNS.Service.TaiLieuDinhKemService
{
    public interface ITaiLieuDinhKemService : IEntityService<TaiLieuDinhKem>
    {
        List<TaiLieuDinhKem> GetListTaiLieuAllByType(string LoaiTaiLieu, long itemId);
        TaiLieuDinhKem GetTaiLieuFirstByType(string LoaiTaiLieu, long itemId);
        JsonResultBO SaveMultiFile(string ITEM_TYPE, long ITEM_ID, List<HttpPostedFileBase> lstFile, List<string> NameFile, string AllowExtention, int? maxSize, string folder, string pathSave, long? userID);
    }
}
