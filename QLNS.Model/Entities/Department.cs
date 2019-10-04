using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("Department")]
    public class Department : AuditableEntity<long>
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Code { get; set; }

        public long? ParentId { get; set; }

        public long? Priority { get; set; }

        public int Level { get; set; }
    }
}
