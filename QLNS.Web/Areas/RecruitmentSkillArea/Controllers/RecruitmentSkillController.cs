using CommonHelper.String;
using log4net;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.RecruitmentSkillDetailService;
using QLNS.Service.RecruitmentSkillService;
using QLNS.Service.RecruitmentSkillService.DTO;
using QLNS.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static QLNS.Web.Areas.RecruitmentSkillArea.Models.RecruitmentSkillViewModel;

namespace QLNS.Web.Areas.RecruitmentSkillArea.Controllers
{
    public class RecruitmentSkillController : BaseController
    {
        IRecruitmentSkillService recruitmentSkillService;
        IRecruitmentSkillDetailService recruitmentSkillDetailService;
        ILog logger;

        public RecruitmentSkillController(
            IRecruitmentSkillService recruitmentSkillService,
            IRecruitmentSkillDetailService recruitmentSkillDetailService,
            ILog logger)
        {
            this.recruitmentSkillDetailService = recruitmentSkillDetailService;
            this.recruitmentSkillService = recruitmentSkillService;
            this.logger = logger;
        }

        // GET: RecruitmentSkillArea/RecruitmentSkill
        public ActionResult Index()
        {
            var searchModel = new RecruitmentSkillSearchDTO();
            SessionManager.SetValue("RecruitmentSkillSearch", new RecruitmentSkillSearchDTO());
            RecruitmentSkillIndexViewModel viewModel = new RecruitmentSkillIndexViewModel()
            {
                GroupData = recruitmentSkillService.GetDataByPage(searchModel),
            };
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult GetData(int indexPage, string sortQuery, int pageSize)
        {
            var searchModel = SessionManager.GetValue("RecruitmentSkillSearch") as RecruitmentSkillSearchDTO;
            if (searchModel == null)
            {
                searchModel = new RecruitmentSkillSearchDTO();
            }
            if (!string.IsNullOrEmpty(sortQuery))
            {
                searchModel.sortQuery = sortQuery;
            }
            if (pageSize > 0)
            {
                searchModel.pageSize = pageSize;
            }
            SessionManager.SetValue("RecruitmentSkillSearch", searchModel);
            var data = recruitmentSkillService.GetDataByPage(searchModel, indexPage, pageSize);
            return Json(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SearchData(FormCollection form)
        {
            var searchModel = SessionManager.GetValue("RecruitmentSkillSearch") as RecruitmentSkillSearchDTO;

            if (searchModel == null)
            {
                searchModel = new RecruitmentSkillSearchDTO();
                searchModel.pageSize = 20;
            }

            searchModel.QueryTitle = form["QUERY_TITLE"];
            SessionManager.SetValue("RecruitmentSkillSearch", searchModel);
            var data = recruitmentSkillService.GetDataByPage(searchModel, 1);
            return Json(data);
        }

        public PartialViewResult Edit(long id = 0)
        {
            RecruitmentSkill entity = recruitmentSkillService.GetById(id) ?? new RecruitmentSkill();
            var groupSkillIds = entity.Skills.ToListNumber<long>(',').Cast<object>().ToList();
            RecruitmentKillEditViewModel viewModel = new RecruitmentKillEditViewModel()
            {
                Id = entity.Id,
                Title = entity.Title,
                Skills = entity.Skills.ToListNumber<long>(',').ToArray(),
                GroupSkills = recruitmentSkillDetailService.GetDropDownMultiple("Name", "Id", groupSkillIds)
            };
            return PartialView("_EditPartial", viewModel);
        }

        [HttpPost]
        public JsonResult Save(RecruitmentKillEditViewModel model)
        {
            JsonResultBO result = new JsonResultBO(true);
            try
            {
                if (ModelState.IsValid)
                {
                    if(model.Id == 0)
                    {
                        RecruitmentSkill entity = new RecruitmentSkill()
                        {
                            Title = model.Title,
                            Skills = string.Join(",", model.Skills)
                        };
                        recruitmentSkillService.Create(entity);
                        logger.InfoFormat("Thêm mới nhóm kỹ năng {0}", model.Title);
                    }
                    else
                    {
                        RecruitmentSkill entity = recruitmentSkillService.GetById(model.Id);
                        entity.Title = model.Title;
                        entity.Skills = string.Join(",", model.Skills);
                        recruitmentSkillService.Update(entity);

                        logger.InfoFormat("Cập nhật nhóm kỹ năng {0}", model.Title);
                    }
                }
                else
                {
                    result.Status = false;
                    result.Message = ModelState.GetErrors();
                }
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = "Không thể cập nhật nhóm kỹ năng";
                logger.Error("Lỗi cập nhật nhóm kỹ năng", ex);
            }
            return Json(result);
        }


        [HttpDelete]
        public JsonResult Delete(long id)
        {
            JsonResultBO result = new JsonResultBO(true);
            try
            {
                var entity = recruitmentSkillService.GetById(id);
                if (entity != null)
                {
                    recruitmentSkillService.Delete(entity);
                    result.Message = "Xóa nhóm kỹ năng thành công";
                }
                else
                {
                    result.Status = false;
                    result.Message = "Nhóm kỹ năng không tồn tại";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Xóa nhóm kỹ năng không thành công";
                logger.Error("Xóa nhóm kỹ năng thành công", ex);
            }
            return Json(result);
        }
    }
}