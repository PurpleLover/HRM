using CommonHelper.Validation;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.OperationService.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Web.Areas.OperationArea.Models
{
    public class OperationViewModel
    {
        public class OperationIndexViewModel
        {
            public int ModuleId { get; set; }
            public PageListResultBO<OperationDTO> GroupData { set; get; }
            public Module module { get; set; }
        }

        public class OperationEditViewModel
        {
            public long Id { get; set; }
            public int ModuleId { get; set; }

            [RequiredExtend]
            [StringLengthExtends(250)]
            [HTMLInjection]
            [SpecialCharacter]
            public string Name { get; set; }

            [RequiredExtend]
            [StringLengthExtends(250)]
            public string URL { get; set; }

            [RequiredExtend]
            [StringLengthExtends(250)]
            [HTMLInjection]
            //[SpecialCharacter]
            public string Code { get; set; }

            [StringLengthExtends(250)]
            [HTMLInjection]
            public string Css { get; set; }

            [Required]
            public bool IsShow { get; set; }

            [Numeric]
            public string Order { get; set; }
        }
    }
}