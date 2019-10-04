using CommonHelper.String;
using log4net;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.Constant;
using QLNS.Service.DM_NhomDanhmucService;
using QLNS.Service.RecruitmentSkillDetailService;
using QLNS.Service.RecruitmentSkillDetailService.DTO;
using QLNS.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static QLNS.Web.Areas.RecruitmentSkillDetailArea.Models.RecruitmentSkillDetailViewModel;

namespace QLNS.Web.Areas.RecruitmentSkillDetailArea.Controllers
{
    public class RecruitmentSkillDetailController : BaseController
    {
        IRecruitmentSkillDetailService recruitmentSkillDetailService;
        IDM_NhomDanhmucService nhomDanhMucService;
        ILog logger;

        public RecruitmentSkillDetailController(
            IRecruitmentSkillDetailService recruitmentSkillDetailService,
            IDM_NhomDanhmucService nhomDanhMucService,
            ILog logger)
        {
            this.recruitmentSkillDetailService = recruitmentSkillDetailService;
            this.nhomDanhMucService = nhomDanhMucService;
            this.logger = logger;
        }

        // GET: RecruitmentSkillDetailArea/RecruitmentSkillDetail
        public ActionResult Index()
        {
            var searchModel = new RecruitmentSkillDetailSearchDTO();
            SessionManager.SetValue("RecruitmentSkillDetailSearch", new RecruitmentSkillDetailSearchDTO());
            RecruitmentSkillDetailIndexViewModel viewModel = new RecruitmentSkillDetailIndexViewModel()
            {
                GroupData = recruitmentSkillDetailService.GetDataByPage(searchModel),
                GroupDataType = ConstantExtension.GetDropdownData<DataTypeConstant>()
            };
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult GetData(int indexPage, string sortQuery, int pageSize)
        {
            var searchModel = SessionManager.GetValue("RecruitmentSkillDetailSearch") as RecruitmentSkillDetailSearchDTO;
            if (searchModel == null)
            {
                searchModel = new RecruitmentSkillDetailSearchDTO();
            }
            if (!string.IsNullOrEmpty(sortQuery))
            {
                searchModel.sortQuery = sortQuery;
            }
            if (pageSize > 0)
            {
                searchModel.pageSize = pageSize;
            }
            SessionManager.SetValue("RecruitmentSkillDetailSearch", searchModel);
            var data = recruitmentSkillDetailService.GetDataByPage(searchModel, indexPage, pageSize);
            return Json(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SearchData(FormCollection form)
        {
            var searchModel = SessionManager.GetValue("RecruitmentSkillDetailSearch") as RecruitmentSkillDetailSearchDTO;

            if (searchModel == null)
            {
                searchModel = new RecruitmentSkillDetailSearchDTO();
                searchModel.pageSize = 20;
            }

            searchModel.QueryName = form["QUERY_NAME"];
            searchModel.QueryType = form["QUERY_TYPE"].ToNullableNumber<int>();
            SessionManager.SetValue("RoleSearch", searchModel);
            var data = recruitmentSkillDetailService.GetDataByPage(searchModel, 1);
            return Json(data);
        }

        public PartialViewResult Edit(long id = 0)
        {
            RecruitmentSkillDetailEditViewModel viewModel = new RecruitmentSkillDetailEditViewModel();
            viewModel.EditEntity = recruitmentSkillDetailService.GetById(id) ?? new RecruitmentSkillDetail();
            viewModel.GroupDataType = ConstantExtension.GetDropdownData<DataTypeConstant>(viewModel.EditEntity.DataType.ToString());
            return PartialView("_EditPartial", viewModel);
        }

        [HttpPost]
        public JsonResult Save(FormCollection form)
        {
            long id = form["Id"].ToNumber<long>();
            JsonResultBO result = new JsonResultBO(true);
            try
            {
                RecruitmentSkillDetail entity = recruitmentSkillDetailService.GetById(id) ?? new RecruitmentSkillDetail();
                entity.Name = form["Name"].Trim();
                entity.DataType = form["DataType"].ToNumber<int>();
                if (entity.DataType == DataTypeConstant.CATEGORY)
                {
                    entity.CategoryId = form["CategoryId"].ToNullableNumber<int>();
                }
                else if (entity.DataType == DataTypeConstant.NUMBER)
                {
                    entity.AbsoluteNumber = form["AbsoluteNumber"].ToNullableNumber<int>();
                }
                entity.CreatedDate = DateTime.Now;
                entity.UpdatedDate = DateTime.Now;
                if (entity.Id == 0)
                {
                    recruitmentSkillDetailService.Create(entity);
                    result.Message = "Thêm mới kỹ năng thành công";
                }
                else
                {
                    recruitmentSkillDetailService.Update(entity);
                    result.Message = "Cập nhật kỹ năng thành công";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// </summary>
        /// <param name="id">mã chi tiết kỹ năng</param>
        /// <returns></returns>
        public PartialViewResult GetDataTypeConfig(int id)
        {
            var entity = recruitmentSkillDetailService.GetById(id) ?? new RecruitmentSkillDetail();
            if (entity.DataType == DataTypeConstant.CATEGORY)
            {
                var data = nhomDanhMucService.GetDropdown("GroupName", "Id", (long)entity.CategoryId);
                return PartialView("_CategoryConfig", data);
            }
            else if (entity.DataType == DataTypeConstant.NUMBER)
            {
                var data = entity.AbsoluteNumber.GetValueOrDefault();
                return PartialView("_NumberConfig", data);
            }
            else if (entity.DataType == DataTypeConstant.TEXT)
            {
                return PartialView("_TextConfig");
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeId">loại cấu hình</param>
        /// <returns></returns>
        public PartialViewResult GetDataTypeConfigByType(int typeId)
        {
            if (typeId == DataTypeConstant.CATEGORY)
            {
                var data = nhomDanhMucService.GetDropdown("GroupName", "Id");
                return PartialView("_CategoryConfig", data);
            }
            else if (typeId == DataTypeConstant.NUMBER)
            {
                var data = 0;
                return PartialView("_NumberConfig", data);
            }
            else if (typeId == DataTypeConstant.TEXT)
            {
                return PartialView("_TextConfig");
            }
            return null;
        }

        [HttpDelete]
        public JsonResult Delete(long id)
        {
            JsonResultBO result = new JsonResultBO(true);
            try
            {
                var entity = recruitmentSkillDetailService.GetById(id);
                if (entity != null)
                {
                    recruitmentSkillDetailService.Delete(entity);
                    result.Message = "Xóa kỹ năng thành công";
                }
                else
                {
                    result.Status = false;
                    result.Message = "Kỹ năng không tồn tại";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Xóa kỹ năng không thành công";
                logger.Error("Xóa kỹ năng không thành công", ex);
            }
            return Json(result);
        }
    }
}