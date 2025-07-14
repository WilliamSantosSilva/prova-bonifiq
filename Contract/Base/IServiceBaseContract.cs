using System.Linq.Expressions;
using ProvaPub.Models;

namespace ProvaPub.Contract.Base
{
    public interface IServiceBaseContract<TEntity> where TEntity : class
    {
        public Task<TEntity> GetById(int id);
        Task<TEntity> Add(TEntity model);
        Task<PagedResult<TEntity>> GetPaged(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, object>> orderBy = default, // Campo para ordenação
            Expression<Func<TEntity, bool>> where = default, // Filtro opcional
            Expression<Func<TEntity, object>>[] navigationProperties = default, // Eager loading opcional
            bool orderByDescending = false);

        Task<int?> GetCount(Expression<Func<TEntity, bool>> where);
    }
}