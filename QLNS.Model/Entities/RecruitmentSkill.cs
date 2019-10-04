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
    /// bảng nhóm kỹ năng
    /// </summary>
    [Table("RecruitmentSkill")]
    public class RecruitmentSkill : AuditableEntity<long>
    {
        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        /// <summary>
        /// danh sách kỹ năng
        /// </summary>
        [Required]
        public string Skills { get; set; }
    }
}
