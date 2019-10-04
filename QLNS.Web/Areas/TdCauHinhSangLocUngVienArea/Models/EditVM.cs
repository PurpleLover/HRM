using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Web.Areas.TdCauHinhSangLocUngVienArea.Models
{
    public class EditVM
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thông tin này")]
        [MinLength(3, ErrorMessage = "Tối thiểu 3 kí tự")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 kí tự")]
        public string Title { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }

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
