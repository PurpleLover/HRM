using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using QLNS.Model;
namespace QLNS.Repository
{
    public interface IGenericRepository<T> where T : class
    {

        IEnumerable<T> GetAll();
        IQueryable<T> GetAllAsQueryable();
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        T Delete(T entity);
        void Edit(T entity);
        void Save();
        T GetById(object id);
        void DeleteRange(IEnumerable<T> entities);
        List<SelectListItem> GetDropdown(string displayMember, string valueMember, object selected = null);
        List<SelectListItem> GetDropDownMultiple(string displayMember, string valueMember, List<object> selected = null);
        List<object> GetListFieldValue(string fieldName);
        List<SelectListItem> GetDropdownFields(object selected = null);
        List<T> GetEntitiesByFieldValue(string fieldName, object value);
        List<T> GetEntitiesByMultipleFieldValue(params KeyValuePair<string, object>[] groupKeyValue);
    }
}
