using QLNS.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Web.Areas.OperationArea.Models
{
    public class AddMenuViewModel
    {
        public List<SelectListItem> ListPermissionCode { get; set; }
        public OperationViewModel.OperationEditViewModel EditViewModel { get; set; }
        public List<SelectListItem> ListModule { get; set; }
    }
}