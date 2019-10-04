using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("TD_CauHinhSangLocUngVien")]
    public class TD_CauHinhSangLocUngVien : AuditableEntity<long>
    {
        [Required]
        [StringLength(250)]
        public string Title { get; set; }
        [Required]
        public string Type { get; set; }
        public string Note { get; set; }
        public long? DotTuyenDungId { get; set; }
        public int? Priority { get; set; }

        #region Type == Interview
        public DateTime? InterviewDate { get; set; }
        public long? InterviewerId { get; set; } 
        public string InterviewerName { get; set; }
        #endregion

        #region Type == Test
        public long? TestId { get; set; }
        public string TestApprovedLimit { get; set; }
        #endregion
    }
}
