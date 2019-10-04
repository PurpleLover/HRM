using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using QLNS.Model;

namespace QLNS.Service
{
    public interface IEntityService<T> : IService
     where T : class
    {
        void Create(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();      
        void Update(T entity);
        T GetById(object id);
        List<SelectListItem> GetDropdown(string displayMember, string valueMember, object selected = null);
        List<SelectListItem> GetDropDownMultiple(string displayMember, string valueMember, List<object> selected = null);
        List<object> GetListFieldValue(string fieldName);
        List<SelectListItem> GetDropdownFields(object selected = null);
        List<T> GetEntitiesByFieldValue(string fieldName, object value);
        List<T> GetEntitiesByMultipleFieldValue(params KeyValuePair<string, object>[] groupKeyValue);
    }
}
