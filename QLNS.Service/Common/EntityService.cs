using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLNS.Model;
using QLNS.Repository;
using log4net;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace QLNS.Service
{
    public abstract class EntityService<T> : IEntityService<T> where T : class
    {
        IUnitOfWork _unitOfWork;
        IGenericRepository<T> _repository;
       

        public EntityService(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }


        public virtual void Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _repository.Add(entity);
            _unitOfWork.Commit();
        }


        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Edit(entity);
            _unitOfWork.Commit();
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Delete(entity);
            _unitOfWork.Commit();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual T GetById(object id)
        {
            return _repository.GetById(id);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _repository.DeleteRange(entities);
            _unitOfWork.Commit();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _repository.GetAllAsQueryable().Where(predicate);
        }

        public List<SelectListItem> GetDropdown(string displayMember, string valueMember, object selected = null)
        {
            return _repository.GetDropdown(displayMember, valueMember, selected);
        }

        public List<SelectListItem> GetDropDownMultiple(string displayMember, string valueMember, List<object> selected = null)
        {
            return _repository.GetDropDownMultiple(displayMember, valueMember, selected);
        }

        public List<object> GetListFieldValue(string fieldName)
        {
            return _repository.GetListFieldValue(fieldName);
        }

        public List<SelectListItem> GetDropdownFields(object selected = null)
        {
            return _repository.GetDropdownFields(selected);
        }

        public List<T> GetEntitiesByFieldValue(string fieldName, object value)
        {
            return _repository.GetEntitiesByFieldValue(fieldName, value);
        }

        public List<T> GetEntitiesByMultipleFieldValue(params KeyValuePair<string, object>[] groupKeyValue)
        {
            return _repository.GetEntitiesByMultipleFieldValue(groupKeyValue);
        }
    }
}
