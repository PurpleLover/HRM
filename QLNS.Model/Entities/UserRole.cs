using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("UserRole")]
    public class UserRole: AuditableEntity<long>
    {
        public long UserId { get; set; }
        public int RoleId { get; set; }

    }
}
