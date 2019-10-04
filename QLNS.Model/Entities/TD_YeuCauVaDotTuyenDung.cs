using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("TD_YeuCauVaDotTuyenDung")]
    public class TD_YeuCauVaDotTuyenDung : AuditableEntity<long>
    {
        public int IdYeuCauTuyenDung { get; set; }
        public int IdDotTuyenDung { get; set; }
    }
}
