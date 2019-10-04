using QLNS.Web.Filters;
using QLNS.Web.Areas.TdQuanLyMauTestArea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLNS.Service.TD_QuanLyMauTestService;
using QLNS.Service.DM_DulieuDanhmucService;
using log4net.Core;
using AutoMapper;
using QLNS.Model.Entities;
using System.Web.Hosting;
using QLNS.Service.TD_QuanLyMauTestService.DTO;
using log4net;
using CommonHelper.String;
using QLNS.Service.Common;

namespace QLNS.Web.Areas.TdQuanLyMauTestArea.Controllers
{
    public class TdQuanLyMauTestController : BaseController
    {
        // GET: TdQuanLyMauTestArea/TdQuanLyMauTest
        ITD_QuanLyMauTestService _quanLyMauTestService;
        IDM_DulieuDanhmucService _dulieuDanhmucService;

        ILog _logger;
        IMapper _mapper;
        string SessionSearchString = "TdQuanLyMauTestSearchModel";

        public TdQuanLyMauTestController(ITD_QuanLyMauTestService quanLyMauTestService, IDM_DulieuDanhmucService dulieuDanhmucService, ILog logger, IMapper mapper)
        {
            _quanLyMauTestService = quanLyMauTestService;
            _dulieuDanhmucService = dulieuDanhmucService;
            _logger = logger;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var data = _quanLyMauTestService.GetDaTaByPage(null);
            ViewBag.DropdownCategory = _dulieuDanhmucService.GetDropdownlist("THELOAI_BIEUMAU", "");
            SessionManager.SetValue(SessionSearchString, null);
            return View(data);
        }

        [HttpPost]
        public JsonResult GetData(long groupid, int indexPage, string sortQuery, int pageSize)
        {
            var searchModel = SessionManager.GetValue(SessionSearchString) as QuanLyMauTestSearchDTO;
            if (searchModel == null)
            {
                searchModel = new QuanLyMauTestSearchDTO();
            }
            if (!string.IsNullOrEmpty(sortQuery))
            {
                searchModel.sortQuery = sortQuery;
            }
            if (pageSize > 0)
            {
                searchModel.pageSize = pageSize;
            }
            SessionManager.SetValue(SessionSearchString, searchModel);
            var data = _quanLyMauTestService.GetDaTaByPage(searchModel, indexPage, pageSize);
            return Json(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SearchData(FormCollection form)
        {
            var searchModel = SessionManager.GetValue(SessionSearchString) as QuanLyMauTestSearchDTO;

            if (searchModel == null)
            {
                searchModel = new QuanLyMauTestSearchDTO();
                searchModel.pageSize = 20;
            }

            searchModel.QueryName = form["QueryName"];
            searchModel.QueryCategoryList = form["QueryCategoryList"].ToListStringLower(',');
            SessionManager.SetValue(SessionSearchString, searchModel);
            var data = _quanLyMauTestService.GetDaTaByPage(searchModel, 1, searchModel.pageSize);
            return Json(data);
        }

        public PartialViewResult Create()
        {
            var model = new CreateVM();
            ViewBag.DropdownCategory = _dulieuDanhmucService.GetDropdownlist("THELOAI_BIEUMAU", "");
            return PartialView("_CreatePartial", model);
        }

        [HttpPost]
        public JsonResult Create(CreateVM model, HttpPostedFileBase File)
        {
            var result = new JsonResultBO(true);
            try
            {
                if (ModelState.IsValid)
                {
                    var EntityModel = _mapper.Map<TD_QuanLyMauTest>(model);
                    if (File != null && File.ContentLength > 0)
                    {
                        var saveRes = _quanLyMauTestService.SaveTemplateFile(new SaveFileModel()
                        {
                            File = File,
                            Name = EntityModel.FileName,
                            ExtensionList = null,
                            MaxSize = null,
                            Path = HostingEnvironment.MapPath("/Uploads"),
                            Folder = "FileMauTest"
                        }, EntityModel.Category);
                        if (!saveRes.Status)
                        {
                            throw new Exception("Không lưu được tệp biểu mẫu");
                        }
                    }
                    else
                    {
                        throw new Exception("Vui lòng đăng tải tệp biểu mẫu");
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                _logger.Error("Lỗi khi lưu biểu mẫu test mới", ex);
            }
            return Json(result);
        }

        public PartialViewResult Edit(long id)
        {
            var existedModel = _quanLyMauTestService.GetById(id);
            if (existedModel == null)
            {
                throw new HttpException(404, "Không tìm thấy thông tin");
            }
            var model = _mapper.Map<TD_QuanLyMauTest, EditVM>(existedModel);
            ViewBag.DropdownCategory = _dulieuDanhmucService.GetDropdownlist("THELOAI_BIEUMAU", model.Category).AddDefault("--Chọn thể loại--");
            return PartialView("_EditPartial", model);
        }

        [HttpPost]
        public JsonResult Edit(EditVM model, HttpPostedFileBase File)
        {
            var result = new JsonResultBO(true);
            try
            {
                if (ModelState.IsValid)
                {
                    var EntityModel = _quanLyMauTestService.GetById(model.Id);
                    EntityModel = _mapper.Map(model, EntityModel);
                    if (File != null && File.ContentLength > 0)
                    {
                        var saveRes = _quanLyMauTestService.SaveTemplateFile(new SaveFileModel()
                        {
                            File = File,
                            Name = EntityModel.FileName,
                            ExtensionList = null,
                            MaxSize = null,
                            Path = HostingEnvironment.MapPath("/Uploads"),
                            Folder = "FileMauTest"
                        }, EntityModel.Category);
                        if (!saveRes.Status)
                        {
                            throw new Exception("Không lưu được tệp biểu mẫu");
                        }
                    }
                    else
                    {
                        throw new Exception("Vui lòng đăng tải tệp biểu mẫu");
                    }
                }
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                _logger.Error("Lỗi khi cập nhật biểu mẫu test", ex);
            }
            return Json(result);
        }

        [HttpDelete]
        public JsonResult Delete(long id)
        {
            var result = new JsonResultBO(true);
            try
            {
                var entity = _quanLyMauTestService.GetById(id);
                if (entity != null)
                {
                    _quanLyMauTestService.Delete(entity);
                    result.Message = "Xóa biểu mẫu thành công";
                    _logger.Info(result.Message);
                }
                else
                {
                    result.Status = false;
                    result.Message = "Biểu mẫu không tồn tại";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Xóa biểu mẫu không thành công";
                _logger.Error("Xóa danh mục không thành công", ex);
            }
            return Json(result);
        }
    }
}