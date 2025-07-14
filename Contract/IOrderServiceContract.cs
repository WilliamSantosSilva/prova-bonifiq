using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProvaPub.Contract.Base;
using ProvaPub.Models;

namespace ProvaPub.Contract
{
    public interface IOrderServiceContract : IServiceBaseContract<Entity.Order>
    {
        Task<Models.OrderModel> PayOrder(string paymentMethod, decimal paymentValue, int customerId);
    }
}