using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ProvaPub.Contract.Base;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services.Base
{
    public class ServiceBase <TEntity> : IServiceBaseContract<TEntity>
        where TEntity : Entity.Base.BaseEntity
    {
        protected readonly IRepositoryBase<TEntity> _repository;
        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<TEntity> Add(TEntity model)
        {
            return await _repository.Add(model);
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<int?> GetCount(Expression<Func<TEntity, bool>> where)
        {
            return await _repository.GetCount(where);
        }

        public async Task<PagedResult<TEntity>> GetPaged(int pageNumber, int pageSize, Expression<Func<TEntity, object>> orderBy = null, Expression<Func<TEntity, bool>> where = null, Expression<Func<TEntity, object>>[] navigationProperties = null, bool orderByDescending = false)
        {
            return await _repository.GetPaged(pageNumber, pageSize, orderBy, where, navigationProperties, orderByDescending);
        }
    }
}