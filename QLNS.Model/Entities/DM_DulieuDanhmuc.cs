using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("DM_DulieuDanhmuc")]
    public class DM_DulieuDanhmuc : AuditableEntity<long>
    {
        public long? GroupId { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [Required]
        [StringLength(250)]
        public string Code { get; set; }
        [StringLength(250)]
        public string Note { get; set; }
        public int? Priority { get; set; }
    }
}
