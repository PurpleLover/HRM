using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("Operation")]
    public class Operation : AuditableEntity<long>
    {
        [Required]
        public int ModuleId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string URL { get; set; }

        [Required]
        public string Code { get; set; }

        [StringLength(250)]
        public string Css { get; set; }

        [Required]
        public bool IsShow { get; set; }

        public int Order { get; set; }
    }
}
