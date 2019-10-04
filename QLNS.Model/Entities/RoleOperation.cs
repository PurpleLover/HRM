using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("RoleOperation")]
    public class RoleOperation : AuditableEntity<long>
    {
        [Required]
        public int RoleId { get; set; }

        [Required]
        public long OperationId { get; set; }

        [Required]
        public int IsAccess { get; set; }
    }
}
