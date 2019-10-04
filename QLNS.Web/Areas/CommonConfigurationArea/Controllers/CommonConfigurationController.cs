using log4net;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.CommonConfigurationService;
using QLNS.Service.CommonConfigurationService.DTO;
using QLNS.Web.Areas.CommonConfigurationArea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Web.Areas.CommonConfigurationArea.Controllers
{
    public class CommonConfigurationController : Controller
    {
        // GET: CommonConfigurationArea/CommonConfiguration
        ICommonConfigurationService _commonConfigurationService;
        ILog _ILog;
        const string SessionSearchString = "CommonConfigurationSearch";

        public CommonConfigurationController(ICommonConfigurationService commonConfigurationService, ILog ILog)
        {
            _commonConfigurationService = commonConfigurationService;
            _ILog = ILog;
        }
        public ActionResult Index()
        {
            var searchModel = new CommonConfigurationDTO();
            SessionManager.SetValue(SessionSearchString, null);
            var data = _commonConfigurationService.GetDataByPage(null);
            return View("Index", data);
        }

        [HttpPost]
        public JsonResult GetData(int indexPage, string sortQuery, int pageSize)
        {
            var searchModel = SessionManager.GetValue(SessionSearchString) as CommonConfigurationSearchDTO;
            if (searchModel == null)
            {
                searchModel = new CommonConfigurationSearchDTO();
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
            var data = _commonConfigurationService.GetDataByPage(searchModel, indexPage, pageSize);
            return Json(data);
        }

        public PartialViewResult Create()
        {
            var model = new CreateVM();
            return PartialView("_CreatePartial", model);
        }
        [HttpPost]
        public JsonResult Create(CreateVM model)
        {
            var result = new JsonResultBO(true);
            try
            {
                if (ModelState.IsValid)
                {
                    if (!String.IsNullOrEmpty(model.ConfigCode) && _commonConfigurationService.CheckCodeExisted(model.ConfigCode.ToUpper()))
                    {
                        throw new Exception(String.Format("Mã cấu hình {0} đã tồn tại!", model.ConfigCode.ToUpper()));
                    }
                    var savedModel = new CommonConfiguration();
                    savedModel.ConfigName = model.ConfigName;
                    savedModel.ConfigCode = model.ConfigCode.ToUpper();
                    savedModel.ConfigData = model.ConfigData;
                    _commonConfigurationService.Create(savedModel);
                    _ILog.Info(String.Format("Thêm mới cấu hình {0}", savedModel.ConfigName));
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                _ILog.Error("Lỗi ở cấu hình chung", ex);
            }
            return Json(result);
        }

        public PartialViewResult Edit(long id)
        {
            var model = new EditVM();
            var existedModel = _commonConfigurationService.GetById(id);
            if (existedModel == null)
            {
                throw new HttpException(404, "Không tìm thấy thông tin cấu hình");
            }
            model = new EditVM()
            {
                Id = existedModel.Id,
                ConfigName = existedModel.ConfigName,
                ConfigCode = existedModel.ConfigCode,
                ConfigData = existedModel.ConfigData
            };
            return PartialView("_EditPartial", model);
        }
        [HttpPost]
        public JsonResult Edit(EditVM model)
        {
            var result = new JsonResultBO(true);
            try
            {
                if (ModelState.IsValid)
                {
                    var existedModel = _commonConfigurationService.GetById(model.Id);
                    if (existedModel == null)
                    {
                        throw new Exception("Không tìm thấy cấu hình");
                    }
                    else
                    {
                        if (existedModel.ConfigCode.Equals(model.ConfigCode.ToUpper()))
                        {
                            existedModel.ConfigName = model.ConfigName;
                            existedModel.ConfigData = model.ConfigData;
                            _commonConfigurationService.Update(existedModel);
                        }
                        else if (_commonConfigurationService.CheckCodeExisted(model.ConfigCode.ToUpper()))
                        {
                            throw new Exception(String.Format("Mã cấu hình {0} đã tồn tại", model.ConfigCode.ToUpper()));
                        }
                        else
                        {
                            existedModel.ConfigName = model.ConfigName;
                            existedModel.ConfigCode = model.ConfigCode.ToUpper();
                            existedModel.ConfigData = model.ConfigData;
                            _commonConfigurationService.Update(existedModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Không cập nhật được!";
                _ILog.Error("Lỗi cập nhật chỉnh sửa cấu hình chung", ex);
            }
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SearchData(FormCollection form)
        {
            var searchModel = SessionManager.GetValue(SessionSearchString) as CommonConfigurationSearchDTO;

            if (searchModel == null)
            {
                searchModel = new CommonConfigurationSearchDTO();
                searchModel.pageSize = 20;
            }

            searchModel.QueryName = form["QueryName"];
            searchModel.QueryCode = form["QueryCode"].ToUpper();
            SessionManager.SetValue(SessionSearchString, searchModel);
            var data = _commonConfigurationService.GetDataByPage(searchModel, 1, searchModel.pageSize);
            return Json(data);
        }

        [HttpDelete]
        public JsonResult Delete(long id)
        {
            JsonResultBO result = new JsonResultBO(true);
            try
            {
                CommonConfiguration entity = _commonConfigurationService.GetById(id);
                if (entity != null)
                {
                    _commonConfigurationService.Delete(entity);
                    result.Message = "Xóa cấu hình thành công";
                    _ILog.Info(result.Message);
                }
                else
                {
                    result.Status = false;
                    result.Message = "Cấu hình không tồn tại";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Xóa cấu hình không thành công";
                _ILog.Error("Xóa cấu hình không thành công", ex);
            }
            return Json(result);
        }
    }
}