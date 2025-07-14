using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProvaPub.Contract;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Repository.Customer;
using ProvaPub.Repository.Number;

namespace ProvaPub.Services
{
    public class CustomerService: Base.ServiceBase<Entity.Customer>,ICustomerServiceContract
    {
        private readonly IOrderServiceContract _orderServiceContract;
        public CustomerService(ICustomerRepository repository, IOrderServiceContract orderServiceContract) : base(repository)
        {
            _orderServiceContract = orderServiceContract;
        }

        public async Task<CustomerList> ListCustomers(int page)
        {
            var dataReturn = await base.GetPaged(page, 10);

            return new CustomerList() { HasNext = false, TotalCount = 10, Customers = dataReturn.Items };
        }

        public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
        {
            if (customerId <= 0) throw new ArgumentOutOfRangeException(nameof(customerId));

            if (purchaseValue <= 0) throw new ArgumentOutOfRangeException(nameof(purchaseValue));

            //Business Rule: Non registered Customers cannot purchase
            var customer = await base.GetById(customerId);
            if (customer == null) throw new InvalidOperationException($"Customer Id {customerId} does not exists");

            //Business Rule: A customer can purchase only a single time per month
            var baseDate = DateTime.UtcNow.AddMonths(-1);
            var ordersInThisMonth = await _orderServiceContract.GetCount(s => s.CustomerId == customerId && s.OrderDate >= baseDate);
            if (ordersInThisMonth > 0)
                return false;

            //Business Rule: A customer that never bought before can make a first purchase of maximum 100,00
            var haveBoughtBefore = await base.GetCount(s => s.Id == customerId && s.Orders.Any());
            if (haveBoughtBefore == 0 && purchaseValue > 100)
                return false;

            //Business Rule: A customer can purchases only during business hours and working days
            if (DateTime.UtcNow.Hour < 8 || DateTime.UtcNow.Hour > 18 || DateTime.UtcNow.DayOfWeek == DayOfWeek.Saturday || DateTime.UtcNow.DayOfWeek == DayOfWeek.Sunday)
                return false;


            return true;
        }

    }
}
