using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaPub.Models
{
    public class PagedResult <TEntity>
    {
        public IEnumerable<TEntity> Items { get; set; } // Os itens da página atual
        public int PageNumber { get; set; }           // Número da página atual
        public int PageSize { get; set; }             // Tamanho da página
        public int TotalPages { get; set; }           // Total de páginas
        public int TotalCount { get; set; }           // Total de registros sem paginação
    }
}