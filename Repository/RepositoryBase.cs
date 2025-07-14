using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;

namespace ProvaPub.Repository
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : Entity.Base.BaseEntity
    {
        protected readonly DbContext Db;
        protected DbSet<TEntity> DbSet;

        public RepositoryBase(DbContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        public Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> dbQuery = DbSet;

            if (navigationProperties != null)
            {
                //Apply eager loading
                foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<TEntity, object>(navigationProperty);
            }

            var item = dbQuery
                .Where(where)
                .AsNoTracking() //Don't track any changes for the selected item
                .FirstOrDefaultAsync(where); //Apply where clause

            return item;
        }

        public Task<TEntity> GetById(int id)
        {
            return this.GetSingle(x => x.Id == id, default);
        }

        public async Task<int?> GetCount(Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> dbQuery = DbSet;
            var item = dbQuery
                .CountAsync(where);

            return await item;
        }

        public async Task<TEntity> Add(TEntity model)
        {
            try
            {
                await DbSet.AddAsync(model);
                Db.SaveChanges();

                return model;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(ex.ToString());
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        
        /// <summary>
    /// Recupera uma coleção paginada de entidades, com filtro, eager loading e ordenação.
    /// </summary>
    /// <param name="pageNumber">O número da página a ser retornada (baseado em 1).</param>
    /// <param name="pageSize">O número de itens por página.</param>
    /// <param name="orderBy">Uma expressão para ordenação dos resultados.</param>
    /// <param name="where">Uma expressão para filtrar os resultados (opcional).</param>
    /// <param name="navigationProperties">Propriedades de navegação para eager loading (opcional).</param>
    /// <param name="orderByDescending">Define se a ordenação deve ser descendente (padrão é ascendente).</param>
    /// <returns>Um PagedResult contendo os itens da página e informações de paginação.</returns>
    public async Task<PagedResult<TEntity>> GetPaged(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, object>> orderBy = default, // Campo para ordenação
        Expression<Func<TEntity, bool>> where = default, // Filtro opcional
        Expression<Func<TEntity, object>>[] navigationProperties = default, // Eager loading opcional
        bool orderByDescending = false)
    {
        IQueryable<TEntity> query = DbSet; // Começa com o DbSet (sua tabela)

        // 1. Aplicar Eager Loading (Includes)
        if (navigationProperties != default)
        {
            foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }
        }

        // 2. Aplicar Filtro (Where Clause)
        if (where != default)
        {
            query = query.Where(where);
        }

        // 3. Obter o Total de Registros (ANTES da paginação)
        // Isso é importante para calcular o total de páginas
        var totalCount = await query.CountAsync();

        if (orderBy != default)
        {
            // 4. Aplicar Ordenação
            if (orderByDescending)
            {
                query = query.OrderByDescending(orderBy);
            }
            else
            {
                query = query.OrderBy(orderBy);
            }
        }
        

        // 5. Aplicar Paginação (Skip e Take)
        var skip = (pageNumber - 1) * pageSize; // Calcula quantos itens pular
        var items = await query
                        .Skip(skip)
                        .Take(pageSize)
                        .AsNoTracking() // Não rastreia alterações para os itens recuperados
                        .ToListAsync();

        // 6. Calcular Total de Páginas
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // 7. Retornar o PagedResult
        return new PagedResult<TEntity>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalCount = totalCount
        };
    }
    }
}