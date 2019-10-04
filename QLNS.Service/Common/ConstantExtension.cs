using CommonHelper.ObjectExtention;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QLNS.Service.Common
{
    public static class ConstantExtension
    {
        /// <summary>
        /// Lấy danh sách dropdownlist constant theo class
        /// </summary>
        /// <typeparam name="TConst"></typeparam>
        /// <param name="selectedItem"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetDropdownData<TConst>(string selectedItem = null)
        {
            var result = new List<SelectListItem>();
            var listProperty = typeof(TConst).GetProperties();
            if (listProperty != null)
            {
                foreach (var item in listProperty)
                {
                    if (item.GetGetMethod().IsStatic)
                    {
                        var val = item.GetValue(null).ToString();
                        var name = item.GetAttribute<DisplayNameAttribute>(true).DisplayName;
                        result.Add(new SelectListItem()
                        {
                            Text = name,
                            Value = val,
                            Selected = !string.IsNullOrEmpty(selectedItem) ? val == selectedItem : false
                        });
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Lấy Tên của constant đề hiển thị
        /// </summary>
        /// <typeparam name="TConst"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>

        public static string GetName<TConst>(string value)
        {
            var listProperty = typeof(TConst).GetProperties();
            if (listProperty != null)
            {
                foreach (var item in listProperty)
                {
                    if (item.GetGetMethod().IsStatic)
                    {
                        var val = item.GetValue(null).ToString();
                        var name = item.GetAttribute<DisplayNameAttribute>(true).DisplayName;
                        if (val == value)
                        {
                            return name;
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
