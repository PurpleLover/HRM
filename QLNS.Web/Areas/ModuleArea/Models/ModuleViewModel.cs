using CommonHelper.Validation;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.ModuleService.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Web.Areas.ModuleArea.Models
{
    public class ModuleViewModel
    {
        public class ModuleIndexViewModel
        {
            public PageListResultBO<ModuleDTO> GroupData { get; set; }
        }

        public class ModuleEditViewModel
        {
            public int Id { get; set; }
            [RequiredExtend]
            [StringLengthExtends(250)]
            [HTMLInjection]
            [SpecialCharacter]
            public string Name { set; get; }

            [RegularExpression("([0-9]+)", ErrorMessage = "Vui lòng nhập số")]
            public string Order { get; set; }

            [RequiredExtend]
            public bool IsShow { get; set; }
            public string Icon { get; set; }
            [StringLength(250)]
            public string ClassCss { get; set; }
            [StringLength(250)]
            public string StyleCss { get; set; }
            [Required]
            [StringLength(250)]
            public string Code { get; set; }
        }
    }
}