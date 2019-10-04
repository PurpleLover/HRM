using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("CommonConfiguration")]
    public class CommonConfiguration : AuditableEntity<long>
    {
        [Required]
        [StringLength(250)]
        public string ConfigName { get; set; }
        [Required]
        [StringLength(250)]
        public string ConfigCode { get; set; }
        [Required]
        [StringLength(250)]
        public string ConfigData { get; set; }
    }
}
