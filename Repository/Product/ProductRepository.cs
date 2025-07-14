using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;

namespace ProvaPub.Repository.Product
{
    public class ProductRepository : RepositoryBase<Entity.Product>, IProductRepository
    {
        public ProductRepository(TestDbContext context) : base(context)
        {
        }
    }
}