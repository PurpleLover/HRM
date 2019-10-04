using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("TD_QuanLyMauTest")]
    public class TD_QuanLyMauTest : AuditableEntity<long>
    {
        public string FileName { get; set; }
        public string FileDirectory { get; set; }
        public string Category { get; set; }
        public long? Size { get; set; }
    }
}
