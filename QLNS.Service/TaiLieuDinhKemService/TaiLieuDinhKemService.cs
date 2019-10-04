using AutoMapper;
using log4net;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.TaiLieuDinhKemRepository;
using QLNS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonHelper;
using System.Web;
using CommonHelper.Upload;
using CommonHelper.String;

namespace QLNS.Service.TaiLieuDinhKemService
{
    public class TaiLieuDinhKemService : EntityService<TaiLieuDinhKem>, ITaiLieuDinhKemService
    {
        private ITaiLieuDinhKemRepository _taiLieuDinhKemRepository;
        private ILog _logger;
        private IMapper _mapper;
        public TaiLieuDinhKemService(IUnitOfWork unitOfWork,
            ITaiLieuDinhKemRepository taiLieuDinhKemRepository,
            ILog logger,
            IMapper mapper
            ) : base(unitOfWork, taiLieuDinhKemRepository)
        {
            _taiLieuDinhKemRepository = taiLieuDinhKemRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public List<TaiLieuDinhKem> GetListTaiLieuAllByType(string LoaiTaiLieu, long itemId)
        {
            var query = _taiLieuDinhKemRepository.GetAllAsQueryable().Where(x => x.Item_ID == itemId && x.LoaiTaiLieu == LoaiTaiLieu).ToList();
            return query;
        }
        public TaiLieuDinhKem GetTaiLieuFirstByType(string LoaiTaiLieu, long itemId)
        {
            var query = _taiLieuDinhKemRepository.GetAllAsQueryable().Where(x => x.Item_ID == itemId && x.LoaiTaiLieu == LoaiTaiLieu).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            return query;
        }

        public JsonResultBO SaveMultiFile(string ITEM_TYPE, long ITEM_ID, List<HttpPostedFileBase> lstFile, List<string> NameFile, string AllowExtention, int? maxSize, string folder, string pathSave, long? userID)
        {
            var result = new JsonResultBO(true);
            for (int i = 0; i < lstFile.Count; i++)
            {
                if (lstFile[i] != null)
                {
                    var resultSave = UploadProvider.SaveFile(lstFile[i], NameFile[i].GetFileNameFormart(), AllowExtention, maxSize, folder, pathSave);
                    if (resultSave.status)
                    {
                        var obj = new TaiLieuDinhKem();
                        obj.TenTaiLieu = string.IsNullOrEmpty(NameFile[i]) ? lstFile[i].FileName : NameFile[i];
                        var arrName = lstFile[i].FileName.Split('.');
                        var extention = '.' + arrName[arrName.Length - 1];
                        obj.DinhDangFile = extention;
                        obj.DuongDanFile = resultSave.path;
                        obj.Item_ID = ITEM_ID;
                        obj.LoaiTaiLieu = ITEM_TYPE;
                        obj.NgayPhatHanh = DateTime.Now;
                        obj.SoLuongDownload = 0;
                        obj.UserId = userID;
                        obj.KichThuoc = (lstFile[i].ContentLength / 1024);
                        _taiLieuDinhKemRepository.Add(obj);
                        _taiLieuDinhKemRepository.Save();
                    }
                    else
                    {
                        result.Status = false;
                        result.Message += "Tệp " + (i + 1) + " " + resultSave.message + "<br/>";
                    }
                }
            }

            return result;
        }
    }
}
