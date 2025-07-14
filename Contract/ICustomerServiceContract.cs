using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProvaPub.Contract.Base;
using ProvaPub.Models;

namespace ProvaPub.Contract
{
    public interface ICustomerServiceContract : IServiceBaseContract<Entity.Customer>
    {
        Task<CustomerList> ListCustomers(int page);
        Task<bool> CanPurchase(int customerId, decimal purchaseValue);
    }
}