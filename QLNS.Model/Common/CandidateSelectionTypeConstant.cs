using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Common
{
    public class CandidateSelectionTypeConstant
    {
        [DisplayName("Phỏng vấn")]
        public static string Interview { get; set; } = "Interview";
        [DisplayName("Bài test")]
        public static string Test { get; set; } = "Test";
    }
}
