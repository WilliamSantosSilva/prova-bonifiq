using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProvaPub.Models;

namespace ProvaPub.Contract
{
    public interface IProductServiceContract
    {
        Task<ProductList> ListProducts(int page);
    }
}