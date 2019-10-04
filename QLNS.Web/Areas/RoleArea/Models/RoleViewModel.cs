using CommonHelper.Validation;
using QLNS.Service.Common;
using QLNS.Service.RoleOperationService.DTO;
using QLNS.Service.RoleService.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Web.Areas.RoleArea.Models
{
    public class RoleViewModel
    {
        public class RoleIndexViewModel
        {
            public PageListResultBO<RoleDTO> GroupData { get; set; }
        }

        public class RoleOperationConfigViewModel
        {
            public RoleOperationDTO ConfigureData { get; set; }
        }

        public class RoleEditViewModel
        {
            public int Id { get; set; }
            [RequiredExtend]
            [StringLengthExtends(250)]
            [HTMLInjection]
            [SpecialCharacter]
            public string Name { get; set; }

            [RequiredExtend]
            [StringLengthExtends(250)]
            [HTMLInjection]
            [SpecialCharacter]
            public string Code { get; set; }
        }
    }
}