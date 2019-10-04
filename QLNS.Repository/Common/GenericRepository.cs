using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using QLNS.Model;

namespace QLNS.Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T>
       where T : class
    {
        protected DbContext _entities;
        protected readonly IDbSet<T> _dbset;

        public GenericRepository(DbContext context)
        {
            _entities = context;
            _dbset = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {

            return _dbset.AsEnumerable<T>();
        }
        public virtual IQueryable<T> GetAllAsQueryable()
        {
            return _dbset.AsQueryable<T>();
        }
        public IEnumerable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {

            IEnumerable<T> query = _dbset.Where(predicate).AsEnumerable();
            return query;
        }

        public virtual T Add(T entity)
        {
            return _dbset.Add(entity);
        }

        public virtual T Delete(T entity)
        {
            return _dbset.Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }

        public T GetById(object id)
        {
            return _dbset.Find(id);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            foreach(var item in entities)
            {
                if (_entities.Entry(item).State == EntityState.Detached)
                {
                    _dbset.Attach(item);
                }
                _dbset.Remove(item);
            }
        }

        public List<SelectListItem> GetDropdown(string displayMember, string valueMember, object selected = null)
        {
            Type objType = typeof(T);
            List<SelectListItem> result = new List<SelectListItem>();
            if (string.IsNullOrEmpty(displayMember) == false && string.IsNullOrEmpty(valueMember) == false)
            {
                result = this._dbset.ToList().Select(x => new SelectListItem()
                {
                    Value = objType.GetProperty(valueMember).GetValue(x).ToString(),
                    Text = objType.GetProperty(displayMember).GetValue(x).ToString(),
                    Selected = (selected != null) && selected.Equals(objType.GetProperty(valueMember).GetValue(x))
                }).ToList();
            }
            return result;
        }

        public List<SelectListItem> GetDropDownMultiple(string displayMember, string valueMember, List<object> selected = null)
        {
            Type objType = typeof(T);
            List<SelectListItem> result = new List<SelectListItem>();
            if (string.IsNullOrEmpty(displayMember) == false && string.IsNullOrEmpty(valueMember) == false)
            {
                result = this._dbset.ToList().Select(x => new SelectListItem()
                {
                    Value = objType.GetProperty(valueMember).GetValue(x).ToString(),
                    Text = objType.GetProperty(displayMember).GetValue(x).ToString(),
                    Selected = (selected != null) && selected.Contains(objType.GetProperty(valueMember).GetValue(x))
                }).ToList();
            }
            return result;
        }

        public List<object> GetListFieldValue(string fieldName)
        {
            Type objType = typeof(T);
            List<object> result = null;
            try
            {
                result = this._dbset.ToList()
                    .Select(x => objType.GetProperty(fieldName).GetValue(x)).ToList();
            }
            catch (Exception ex)
            {
                result = new List<object>();
            }
            return result;
        }

        public List<SelectListItem> GetDropdownFields(object selected = null)
        {
            Type objType = typeof(T);
            List<SelectListItem> result = new List<SelectListItem>();
            result = objType.GetProperties().Select(x => new SelectListItem()
            {
                Value = x.Name,
                Text = x.Name,
                Selected = (selected != null) && x.Name.Equals(selected)
            }).ToList();
            return result;
        }

        public List<T> GetEntitiesByFieldValue(string fieldName, object value)
        {
            Type objectType = typeof(T);
            List<T> result = this._dbset.ToList()
                .Where(x => objectType.GetProperty(fieldName)
                .GetValue(x).Equals(value)).ToList();
            return result;
        }

        public List<T> GetEntitiesByMultipleFieldValue(params KeyValuePair<string, object>[] groupKeyValue)
        {
            Type objectType = typeof(T);
            List<T> result = this._dbset.ToList();
            foreach (var pair in groupKeyValue)
            {
                result = result
                    .Where(x => objectType.GetProperty(pair.Key)
                    .GetValue(x).Equals(pair.Value)).ToList();
            }
            return result;
        }
    }
}
