using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ProvaPub.Models;

namespace ProvaPub.Repository
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>>[] navigationProperties);
        Task<TEntity> Add(TEntity model);
        Task<PagedResult<TEntity>> GetPaged(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, object>> orderBy = default, // Campo para ordenação
            Expression<Func<TEntity, bool>> where = default, // Filtro opcional
            Expression<Func<TEntity, object>>[] navigationProperties = default, // Eager loading opcional
            bool orderByDescending = false);

        Task<int?> GetCount(Expression<Func<TEntity, bool>> where);
        Task<TEntity> GetById(int id);
    }
}