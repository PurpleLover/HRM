using AutoMapper;
using CommonHelper.String;
using log4net;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.ModuleService;
using QLNS.Service.ModuleService.DTO;
using QLNS.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static QLNS.Web.Areas.ModuleArea.Models.ModuleViewModel;

namespace QLNS.Web.Areas.ModuleArea.Controllers

/*
 * @author:duynn
 * @create_date: 19/04/2019
 */
{
    public class ModuleController : BaseController
    {
        private readonly IModuleService _moduleService;
        private readonly ILog _Ilog;
        private readonly IMapper _mapper;

        public ModuleController(IModuleService moduleService, ILog Ilog, IMapper mapper)
        {
            _moduleService = moduleService;
            _Ilog = Ilog;
            _mapper = mapper;
        }

        // GET: ModuleArea/Module
        public ActionResult Index()
        {
            var searchModel = new ModuleSearchDTO();
            SessionManager.SetValue("ModuleSearch", new ModuleSearchDTO());
            ModuleIndexViewModel viewModel = new ModuleIndexViewModel()
            {
                GroupData = _moduleService.GetDataByPage(searchModel)
            };
            return View(viewModel);
        }


        [HttpPost]
        public JsonResult GetData(int indexPage, string sortQuery, int pageSize)
        {
            var searchModel = SessionManager.GetValue("ModuleSearch") as ModuleSearchDTO;
            if (searchModel == null)
            {
                searchModel = new ModuleSearchDTO();
            }
            if (!string.IsNullOrEmpty(sortQuery))
            {
                searchModel.sortQuery = sortQuery;
            }
            if (pageSize > 0)
            {
                searchModel.pageSize = pageSize;
            }
            SessionManager.SetValue("ModuleSearch", searchModel);
            var data = _moduleService.GetDataByPage(searchModel, indexPage, pageSize);
            return Json(data);
        }

        public PartialViewResult Edit(long id = 0)
        {
            var viewModel = new ModuleEditViewModel();
            var editEntity = _moduleService.GetById(id) ?? new Module() {IsShow=true };
            viewModel = new ModuleEditViewModel()
            {
                Id = editEntity.Id,
                Name = editEntity.Name,
                IsShow = editEntity.IsShow,
                Order = editEntity.Order.ToString(),
                Code = editEntity.Code,
                StyleCss = editEntity.StyleCss,
                ClassCss = editEntity.ClassCss,
                Icon = editEntity.Icon,
            };
            return PartialView("_EditPartial", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SearchData(FormCollection form)
        {
            var searchModel = SessionManager.GetValue("ModuleSearch") as ModuleSearchDTO;

            if (searchModel == null)
            {
                searchModel = new ModuleSearchDTO();
                searchModel.pageSize = 20;
            }
            searchModel.QueryCode = form["QUERY_CODE"];
            searchModel.QueryName = form["QUERY_NAME"];
            searchModel.QueryIcon = form["QUERY_ICON"];
            searchModel.QueryClassCss = form["QUERY_CLASS_CSS"];
            searchModel.QueryStyleCss = form["QUERY_STYLE_CSS"];
            searchModel.QueryIsShow = !string.IsNullOrEmpty(form["QUERY_SHOW"]) ? (bool?)(int.Parse(form["QUERY_SHOW"]) > 0) : null;
            SessionManager.SetValue("ModuleSearch", searchModel);
            var data = _moduleService.GetDataByPage(searchModel, 1);
            return Json(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Save(ModuleEditViewModel model)
        {
            var result = new JsonResultBO(true);
            try
            {
                if (ModelState.IsValid)
                {

                    if (model.Id <= 0)
                    {
                        if (_moduleService.CheckExistCode(model.Code))
                        {
                            throw new Exception(string.Format("Mã {0} đã tồn tại", model.Code));
                        }
                        Module entity = new Module()
                        {
                            Code = model.Code,
                            ClassCss = model.ClassCss,
                            StyleCss = model.StyleCss,
                            Icon = model.Icon,
                            Name = model.Name,
                            IsShow = model.IsShow,
                            Order = model.Order.ToNumber<int>()
                        };
                        _moduleService.Create(entity);
                        _Ilog.InfoFormat("Thêm mới module {0}", model.Name);
                    }
                    else
                    {
                        if (_moduleService.CheckExistCode(model.Code,model.Id))
                        {
                            throw new Exception(string.Format("Mã {0} đã tồn tại", model.Code));
                        }
                        Module entity = _moduleService.GetById(model.Id);
                        entity.Code = model.Code;
                        entity.ClassCss = model.ClassCss;
                        entity.StyleCss = model.StyleCss;
                        entity.Icon = model.Icon;
                        entity.Name = model.Name;
                        entity.IsShow = model.IsShow;
                        entity.Order = model.Order.ToNumber<int>();
                        _moduleService.Update(entity);

                        _Ilog.InfoFormat("Cập nhật module {0}", model.Name);
                    }


                    return Json(result);
                }
                result.Message = ModelState.GetErrors();
                result.Status = false;
                return Json(result);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                _Ilog.Error("Lỗi cập nhật thông tin Module", ex);
            }
            return Json(result);
        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {
            JsonResultBO result = new JsonResultBO(true);
            try
            {
                Module entity = _moduleService.GetById(id);
                if (entity != null)
                {
                    _moduleService.Delete(entity);
                    result.Message = "Xóa module thành công";
                }
                else
                {
                    result.Status = false;
                    result.Message = "Module không tồn tại";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Xóa module không thành công";
                _Ilog.Error("Xóa module không thành công", ex);
            }
            return Json(result);
        }
    }
}