using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("TD_KetQuaPhongVan")]
    public class TD_KetQuaPhongVan:AuditableEntity<int>
    {
        public int DiemSo { get; set; }
    }
}
