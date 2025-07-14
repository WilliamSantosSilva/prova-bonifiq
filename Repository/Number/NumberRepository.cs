using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;

namespace ProvaPub.Repository.Number
{
    public class NumberRepository : RepositoryBase<Entity.RandomNumber>, INumberRepository
    {
        public NumberRepository(TestDbContext context) : base(context)
        {
        }
    }
}