using CommonHelper.SQL;
using CommonHelper.String;
using log4net;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.RoleOperationService;
using QLNS.Service.RoleService;
using QLNS.Service.RoleService.DTO;
using QLNS.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static QLNS.Web.Areas.RoleArea.Models.RoleViewModel;

namespace QLNS.Web.Areas.RoleArea.Controllers
{
    /// <summary>
    /// @author:duynn
    /// @since: 22/04/2019
    /// </summary>
    public class RoleController : BaseController
    {
        IRoleService _roleService;
        IRoleOperationService _roleOperationService;
        ILog _iLog;

        public RoleController(IRoleService roleService,
            IRoleOperationService roleOperationService,
            ILog Ilog)
        {
            _roleService = roleService;
            _roleOperationService = roleOperationService;
            _iLog = Ilog;
        }
        // GET: ModuleArea/Module
        public ActionResult Index()
        {
            var searchModel = new RoleSearchDTO();
            SessionManager.SetValue("RoleSearch", new RoleSearchDTO());
            RoleIndexViewModel viewModel = new RoleIndexViewModel()
            {
                GroupData = _roleService.GetDataByPage(searchModel)
            };
            return View(viewModel);
        }


        [HttpPost]
        public JsonResult GetData(int indexPage, string sortQuery, int pageSize)
        {
            var searchModel = SessionManager.GetValue("RoleSearch") as RoleSearchDTO;
            if (searchModel == null)
            {
                searchModel = new RoleSearchDTO();
            }
            if (!string.IsNullOrEmpty(sortQuery))
            {
                searchModel.sortQuery = sortQuery;
            }
            if (pageSize > 0)
            {
                searchModel.pageSize = pageSize;
            }
            SessionManager.SetValue("RoleSearch", searchModel);
            var data = _roleService.GetDataByPage(searchModel, indexPage, pageSize);
            return Json(data);
        }

        public PartialViewResult Edit(long id = 0)
        {
            var viewModel = new RoleEditViewModel();
            var editEntity = _roleService.GetById(id) ?? new Role();
            viewModel = new RoleEditViewModel()
            {
                Id = editEntity.Id,
                Name = editEntity.Name,
                Code = editEntity.Code
            };
            return PartialView("_EditPartial", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SearchData(FormCollection form)
        {
            var searchModel = SessionManager.GetValue("RoleSearch") as RoleSearchDTO;

            if (searchModel == null)
            {
                searchModel = new RoleSearchDTO();
                searchModel.pageSize = 20;
            }

            searchModel.QueryName = form["QUERY_NAME"];
            searchModel.QueryCode = form["QUERY_CODE"];
            SessionManager.SetValue("RoleSearch", searchModel);
            var data = _roleService.GetDataByPage(searchModel, 1);
            return Json(data);
        }

        [HttpPost]
        public JsonResult Save(RoleEditViewModel model)
        {
            var result = new JsonResultBO(true);
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id <= 0)
                    {
                        Role entity = new Role()
                        {
                            Name = model.Name,
                            Code = model.Code,
                        };
                        _roleService.Create(entity);
                        _iLog.InfoFormat("Thêm mới vai trò {0}", model.Name);
                    }
                    else
                    {
                        Role entity = _roleService.GetById(model.Id);
                        entity.Name = model.Name;
                        entity.Code = model.Code;
                        _roleService.Update(entity);

                        _iLog.InfoFormat("Cập nhật vai trò {0}", model.Name);
                    }
                    return Json(result);
                }
                result.Status = false;
                result.Message = ModelState.GetErrors();
                return Json(result);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Không cập nhật được";
                _iLog.Error("Lỗi cập nhật thông tin vai trò", ex);
            }
            return Json(result);
        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {
            JsonResultBO result = new JsonResultBO(true);
            try
            {
                Role entity = _roleService.GetById(id);
                if (entity != null)
                {
                    _roleService.Delete(entity);
                    result.Message = "Xóa vai trò thành công";
                }
                else
                {
                    result.Status = false;
                    result.Message = "Vai trò không tồn tại";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Xóa vai trò không thành công";
                _iLog.Error("Xóa vai trò không thành công", ex);
            }
            return Json(result);
        }

        public ActionResult ConfigureOperation(int roleId)
        {
            RoleOperationConfigViewModel viewModel = new RoleOperationConfigViewModel()
            {
                ConfigureData = _roleOperationService.GetConfigureOperation(roleId)
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveConfigureOperation(FormCollection form)
        {
            JsonResultBO result = new JsonResultBO(true);
            int roleId = int.Parse(form["ROLE_ID"]);
            try
            {
                List<RoleOperation> obsoluteData = _roleOperationService.FindBy(x => x.RoleId == roleId).ToList();
                _roleOperationService.DeleteRange(obsoluteData);
                IEnumerable<long> operationIds = form["OPERATION"].ToListNumber<long>();
                List<RoleOperation> configData = new List<RoleOperation>();
                foreach (var operationId in operationIds)
                {
                    RoleOperation config = new RoleOperation()
                    {
                        OperationId = operationId,
                        RoleId = roleId,
                        IsAccess = 1,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };
                    configData.Add(config);
                }
                SQLHelper.BulkInsert<RoleOperation>(configData);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Cập nhật quyền không thành công";
                _iLog.Error($"Cập nhật quyền cho vai trò Id = {roleId} không thành công", ex);
            }
            return Json(result);
        }
    }
}