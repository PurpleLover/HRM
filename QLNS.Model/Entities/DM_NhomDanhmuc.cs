using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("DM_NhomDanhmuc")]
    public class DM_NhomDanhmuc : AuditableEntity<long>
    {
        [Required]
        [StringLength(150)]
        public string GroupName { get; set; }
        [Required]
        [StringLength(150)]
        public string GroupCode { get; set; }
    }
}
