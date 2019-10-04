using log4net;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Service.TD_HoSoUngVienService.DTO;
using System.Linq;
using System.Linq.Dynamic;
using QLNS.Service.Common;
using PagedList;
using QLNS.Service.TD_QuanLyMauTestService.DTO;
using QLNS.Repository.TD_QuanLyMauTestRepository;
using AutoMapper;
using CommonHelper.Upload;
using System.Collections.Generic;
using System.Web;
using CommonHelper.String;

namespace QLNS.Service.TD_QuanLyMauTestService
{
    public class TD_QuanLyMauTestService : EntityService<TD_QuanLyMauTest>, ITD_QuanLyMauTestService
    {
        ITD_QuanLyMauTestRepository _quanLyMauTestRepository;
        ILog _iLog;
        IMapper _imapper;

        public TD_QuanLyMauTestService(IUnitOfWork unitOfWork, ITD_QuanLyMauTestRepository quanLyMauTestRepository, ILog logger, IMapper mapper) :
            base(unitOfWork, quanLyMauTestRepository)
        {
            _quanLyMauTestRepository = quanLyMauTestRepository;
            _iLog = logger;
            _imapper = mapper;
        }

        public PageListResultBO<QuanLyMauTestDTO> GetDaTaByPage(QuanLyMauTestSearchDTO searchModel, int pageIndex = 1, int pageSize = 20)
        {
            var query = _quanLyMauTestRepository.GetAllAsQueryable();

            if (searchModel != null)
            {
                if (!string.IsNullOrEmpty(searchModel.QueryName))
                {
                    query = query.Where(x => x.FileName.Contains(searchModel.QueryName));
                }
                if (searchModel.QueryCategoryList.Count > 0)
                {
                    query = query.Where(x => searchModel.QueryCategoryList.Contains(x.Category));
                }
                if (!string.IsNullOrEmpty(searchModel.sortQuery))
                {
                    query = query.OrderBy(searchModel.sortQuery);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Id);
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.Id);
            }
            var resultmodel = new PageListResultBO<TD_QuanLyMauTest>();
            if (pageSize == -1)
            {
                var dataPageList = query.ToList();
                resultmodel.Count = dataPageList.Count;
                resultmodel.TotalPage = 1;
                resultmodel.ListItem = dataPageList;
            }
            else
            {
                var dataPageList = query.ToPagedList(pageIndex, pageSize);
                resultmodel.Count = dataPageList.TotalItemCount;
                resultmodel.TotalPage = dataPageList.PageCount;
                resultmodel.ListItem = dataPageList.ToList();
            }

            return _imapper.Map<PageListResultBO<QuanLyMauTestDTO>>(resultmodel);
        }

        public JsonResultBO SaveTemplateFile(SaveFileModel saveFileModel, string category)
        {
            var result = new JsonResultBO(true);
            var saveRes = UploadProvider.SaveFile(saveFileModel.File, saveFileModel.Name.GetFileNameFormart(), saveFileModel.ExtensionList, saveFileModel.MaxSize, saveFileModel.Folder, saveFileModel.Path);
            if (saveRes.status)
            {
                var obj = new TD_QuanLyMauTest();
                obj.FileDirectory = saveRes.path;
                obj.FileName = string.IsNullOrEmpty(saveFileModel.Name) ? saveFileModel.File.FileName : saveFileModel.Name;
                obj.Category = category;
                obj.Size = saveFileModel.File.ContentLength / 1024;
                _quanLyMauTestRepository.Add(obj);
                _quanLyMauTestRepository.Save();
            }
            else
            {
                result.Status = false;
                result.Message = saveRes.message;
            }
            return result;
        }
    }
}
