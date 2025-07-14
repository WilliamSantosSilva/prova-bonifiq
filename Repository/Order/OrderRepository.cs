using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository.Order;

namespace ProvaPub.Repository.Number
{
    public class OrderRepository : RepositoryBase<Entity.Order>, IOrderRepository
    {
        public OrderRepository(TestDbContext context) : base(context)
        {
        }
    }
}