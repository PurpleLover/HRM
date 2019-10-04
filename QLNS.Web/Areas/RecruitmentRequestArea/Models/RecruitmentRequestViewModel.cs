using CommonHelper.Validation;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.RecruitmentRequestService.DTO;
using QLNS.Service.RecruitmentSkillService.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Web.Areas.RecruitmentRequestArea.Models
{
    public class RecruitmentRequestViewModel
    {
        public class RecruitmentRequestIndexViewModel
        {
            public IEnumerable<SelectListItem> GroupDepartments { get; set; }
            public IEnumerable<SelectListItem> GroupPositions { get; set; }
            public PageListResultBO<RecruitmentRequestDTO> GroupData { get; set; }
        }

        public class RecruitmentRequestEditViewModel : IValidatableObject
        {
            public long Id { get; set; }

            public int Status { get; set; }

            [RequiredExtend]
            public string DepartmentId { get; set; }

            [RequiredExtend]
            public DateTime? UntilDate { get; set; }

            [RequiredExtend]
            [StringLengthExtends(250)]
            public string Title { get; set; }

            [StringLengthExtends(250)]
            public string Comment { get; set; }

            [StringLengthExtends(250)]
            public string ReasonClose { get; set; }

            /// <summary>
            /// vị trí/chức danh tuyển dụng
            /// </summary>
            [RequiredExtend]
            public string PositionId { get; set; }

            /// <summary>
            /// số lượng dự kiến
            /// </summary>
            [RequiredExtend]
            [Numeric]
            [Range(1, Int32.MaxValue, ErrorMessage = "Số trong khoảng 1 đến 2147483647")]
            public string EstimateQuantity { get; set; }

            public IEnumerable<SelectListItem> GroupDepartments { get; set; }
            public IEnumerable<SelectListItem> GroupPositions { get; set; }

            public IEnumerable<SelectListItem> GroupSkills { get; set; }
            public IEnumerable<SelectListItem> GroupTemplate { get; set; }

            public long[] SkillIds { get; set; }

            public long? TemplateId { get; set; }

            public bool IsChooseFromTemplate { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (IsChooseFromTemplate == true && TemplateId == null)
                {
                    yield return new ValidationResult("Vui lòng chọn biểu mẫu", new[] { "TemplateId" });
                }
                else if(IsChooseFromTemplate != true && SkillIds == null)
                {
                    yield return new ValidationResult("Vui lòng chọn nhóm kỹ năng/yêu cầu", new[] { "SkillIds" });
                }
            }
        }

        public class RecruitmentRequestDetailViewModel
        {
            public RecruitmentRequest Entity { get; set; }
            public RecruitmentRequestDTO EntityDTO { get; set; }
            public IEnumerable<RecruitmentSkillDTO> GroupSkills { get; set; }
            public IEnumerable<ConfigRecruitmentRequest> ConfigsData { get; set; }
        }

        public class RecruitmentSkillsEditViewModel
        {
            public long Id { get; set; }

            [RequiredExtend]
            public string Title { get; set; }
        }
    }
}