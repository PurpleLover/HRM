using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("RecruitmentRequest")]
    public class RecruitmentRequest : AuditableEntity<long>
    {
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        public DateTime UntilDate { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [StringLength(250)]
        public string Comment { get; set; }

        [StringLength(250)]
        public string ReasonClose { get; set; }

        /// <summary>
        /// vị trí/chức danh tuyển dụng
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// số lượng dự kiến
        /// </summary>
        public int EstimateQuantity { get; set; }

        /// <summary>
        /// danh sách nhóm kỹ năng
        /// </summary>
        public string SkillGroups { get; set; }

        /// <summary>
        /// mã biểu mẫu
        /// </summary>
        public long? TemplateId { get; set; }

        /// <summary>
        /// có phải là biểu mẫu hay không?
        /// </summary>
        public bool? IsTemplate { get; set; }
    }
}
