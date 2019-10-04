using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("ConfigRecruitmentRequest")]
    public class ConfigRecruitmentRequest : AuditableEntity<long>
    {
        /// <summary>
        /// mã yêu cầu tuyển dụng
        /// </summary>
        public long? RequestId { get; set; }

        /// <summary>
        /// nhóm kỹ năng
        /// </summary>
        public long? GroupSkillId { get; set; }

        /// <summary>
        /// kỹ năng
        /// </summary>
        public long? SkillId { get; set; }

        /// <summary>
        /// nhóm danh mục
        /// </summary>
        public long? CategoryId { get; set; }

        /// <summary>
        /// dữ liệu nhóm danh mục
        /// </summary>
        public string CategoryData { get; set; }

        /// <summary>
        /// giá trị tuyệt đối
        /// </summary>
        public int? AbsoluteNumber { get; set; }

        /// <summary>
        /// giá trị text box
        /// </summary>
        public string TextValue { get; set; }
    }
}
