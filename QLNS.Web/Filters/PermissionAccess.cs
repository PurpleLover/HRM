using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Web.Filters
{
    public class PermissionAccess : FilterAttribute, IAuthorizationFilter
    {
        //Danh sách user được phép truy cập
        public string Users { get; set; }
        //Danh sách Quyền truy cập
        public string[] Permissions { get; set; }
        /*Kiểu kiểm soát quyền truy cập
         * Only : Chỉ cần tồn tại 1 quyền thì cho phép truy cập
         * All : Phải đáp ứng tất cả các quyền mới được phép truy cập 
        */
        public TypeConstraintConst TypeConstraint { get; set; }
        
        public PermissionAccess()
        {

        }

    }

    public enum  TypeConstraintConst
    {
        Only,
        All
    }
}