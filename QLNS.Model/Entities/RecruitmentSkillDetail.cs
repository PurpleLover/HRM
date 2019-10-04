using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    /// <summary>
    /// bảng: kỹ năng
    /// </summary>
    [Table("RecruitmentSkillDetail")]
    public class RecruitmentSkillDetail : AuditableEntity<long>
    {
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// kiểu dữ liệu
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// giá trị nhóm danh mục
        /// </summary>
        public long? CategoryId { get; set; }

        /// <summary>
        /// dữ liệu nhóm danh mục
        /// </summary>
        public string CategoryData { get; set; }

        /// <summary>
        /// giới hạn trên
        /// </summary>
        public int? UpperNumber { get; set; }

        /// <summary>
        /// giới hạn dưới
        /// </summary>
        public int? LowerNumber { get; set; }

        /// <summary>
        /// giá trị tuyệt đối
        /// </summary>
        public int? AbsoluteNumber { get; set; }

        /// <summary>
        /// giá trị ghi chú
        /// </summary>
        public string TextValue { get; set; }
    }
}
