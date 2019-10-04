using log4net;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.DM_DulieuDanhmucService;
using QLNS.Service.DM_NhomDanhmucService;
using QLNS.Service.DM_NhomDanhmucService.DTO;
using QLNS.Web.Areas.DmNhomDanhmucArea.Models;
using QLNS.Web.Filters;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Web.Areas.DmNhomDanhmucArea.Controllers
{
    public class DmNhomDanhmucController : BaseController
    {
        IDM_NhomDanhmucService _dm_NhomDanhmucService;
        IDM_DulieuDanhmucService _dm_DulieuDanhmucService;
        ILog _ILog;
        const string SessionSearchString = "NhomDanhmucSearch";

        public DmNhomDanhmucController(IDM_NhomDanhmucService dm_NhomDanhmucService, IDM_DulieuDanhmucService dm_DulieuDanhmucService, ILog ILog)
        {
            _dm_NhomDanhmucService = dm_NhomDanhmucService;
            _ILog = ILog;
            _dm_DulieuDanhmucService = dm_DulieuDanhmucService;
        }

        // GET: DmNhomDanhmucArea/DmNhomDanhmuc
        public ActionResult Index()
        {
            var searchModel = new DM_NhomDanhmucSearchDTO();
            SessionManager.SetValue(SessionSearchString, null);
            var data = _dm_NhomDanhmucService.GetDataByPage(null);
            return View("Index", data);
        }

        [HttpPost]
        public JsonResult GetData(int indexPage, string sortQuery, int pageSize)
        {
            var searchModel = SessionManager.GetValue(SessionSearchString) as DM_NhomDanhmucSearchDTO;
            if (searchModel == null)
            {
                searchModel = new DM_NhomDanhmucSearchDTO();
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
            var data = _dm_NhomDanhmucService.GetDataByPage(searchModel, indexPage, pageSize);
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
                    if (!String.IsNullOrEmpty(model.GroupCode) && _dm_NhomDanhmucService.CheckGroupCodeExisted(model.GroupCode.ToUpper()))
                    {
                        throw new Exception(String.Format("Mã nhóm {0} đã tồn tại!", model.GroupCode.ToUpper()));
                    }
                    var nhomDanhmuc = new DM_NhomDanhmuc();
                    nhomDanhmuc.GroupName = model.GroupName;
                    nhomDanhmuc.GroupCode = model.GroupCode.ToUpper();
                    _dm_NhomDanhmucService.Create(nhomDanhmuc);
                    _ILog.Info(String.Format("Thêm mới nhóm danh mục {0}", nhomDanhmuc.GroupName));
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                _ILog.Error("Lỗi ở nhóm danh mục", ex);
            }
            return Json(result);
        }

        public PartialViewResult Edit(long id)
        {
            var model = new EditVM();
            var singleGroup = _dm_NhomDanhmucService.GetById(id);
            if (singleGroup == null)
            {
                throw new HttpException(404, "Không tìm thấy thông tin");
            }
            model = new EditVM()
            {
                Id = singleGroup.Id,
                GroupName = singleGroup.GroupName,
                GroupCode = singleGroup.GroupCode
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
                    var singleGroup = _dm_NhomDanhmucService.GetById(model.Id);
                    if (singleGroup == null)
                    {
                        throw new Exception("Không tìm thấy nhóm danh mục");
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(model.GroupCode))
                        {
                            if (singleGroup.GroupCode.Equals(model.GroupCode))
                            {
                                singleGroup.GroupName = model.GroupName;
                                _dm_NhomDanhmucService.Update(singleGroup);
                            }
                            else if (_dm_NhomDanhmucService.CheckGroupCodeExisted(model.GroupCode.ToUpper()))
                            {
                                throw new Exception(String.Format("Mã nhóm {0} đã tồn tại", model.GroupCode.ToUpper()));
                            }
                            else
                            {
                                singleGroup.GroupCode = model.GroupCode.ToUpper();
                                singleGroup.GroupName = model.GroupName;
                                _dm_NhomDanhmucService.Update(singleGroup);
                            }
                        }
                        else
                        {
                            throw new Exception("Thiếu mã nhóm danh mục");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Không cập nhật được!";
                _ILog.Error("Lỗi cập nhật chỉnh sửa nhóm danh mục", ex);
            }
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SearchData(FormCollection form)
        {
            var searchModel = SessionManager.GetValue(SessionSearchString) as DM_NhomDanhmucSearchDTO;

            if (searchModel == null)
            {
                searchModel = new DM_NhomDanhmucSearchDTO();
                searchModel.pageSize = 20;
            }

            searchModel.QueryName = form["QueryName"];
            searchModel.QueryCode = form["QueryCode"].ToUpper();
            SessionManager.SetValue(SessionSearchString, searchModel);
            var data = _dm_NhomDanhmucService.GetDataByPage(searchModel, 1, searchModel.pageSize);
            return Json(data);
        }

        [HttpDelete]
        public JsonResult Delete(long id)
        {
            JsonResultBO result = new JsonResultBO(true);
            try
            {
                DM_NhomDanhmuc entity = _dm_NhomDanhmucService.GetById(id);
                if (entity != null)
                {
                    List<DM_DulieuDanhmuc> listDulieu = _dm_DulieuDanhmucService.GetListDataByGroupId(id);
                    if (listDulieu != null && listDulieu.Count > 0)
                    {
                        _dm_DulieuDanhmucService.DeleteRange(listDulieu);
                        _ILog.Info(String.Format("Xoá dữ liệu của danh mục {0} thành công", entity.GroupCode));
                    }
                    _dm_NhomDanhmucService.Delete(entity);
                    result.Message = "Xóa nhóm danh mục thành công";
                    _ILog.Info(result.Message);
                }
                else
                {
                    result.Status = false;
                    result.Message = "Nhóm danh mục không tồn tại";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Xóa nhóm danh mục không thành công";
                _ILog.Error("Xóa nhóm danh mục không thành công", ex);
            }
            return Json(result);
        }
    }
}