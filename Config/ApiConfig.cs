using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProvaPub.Contract;
using ProvaPub.Contract.Payment;
using ProvaPub.Repository;
using ProvaPub.Repository.Customer;
using ProvaPub.Repository.Number;
using ProvaPub.Repository.Order;
using ProvaPub.Repository.Product;
using ProvaPub.Services;
using ProvaPub.Services.Payment;
using ProvaPub.Services.Strategy;

namespace ProvaPub.Config
{
    public static class ApiConfig
    {
        public static IServiceCollection RegisterDefaultDataBase(this IServiceCollection services)
        {
            try
            {
                services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<TestDbContext>(options =>
                {
                    options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONN"),
                    options => options.EnableRetryOnFailure());
                });

                return services;
            }
            catch
            {
                throw new Exception("One and more configuration is not found, consulting Enviroments Variable config !!");
            }
        }

        public static IServiceCollection RegisterConfigDI(this IServiceCollection services)
        {
            services.AddTransient<INumberRepository, NumberRepository>();
            services.AddTransient<IRandomServiceContract, RandomService>();

            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ICustomerServiceContract, CustomerService>();

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductServiceContract, ProductService>();

            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderServiceContract, OrderService>();

            services.AddTransient<IPaymentStrategy, PixPaymentStrategy>();
            services.AddTransient<IPaymentStrategy, CreditCardPaymentStrategy>();
            services.AddTransient<IPaymentStrategy, PaypalPaymentStrategy>();
            services.AddSingleton<IPaymentStrategyFactory, PaymentStrategyFactory>();

            return services;
        }
    }
}