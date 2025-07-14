using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProvaPub.Contract;

namespace ProvaPub.Contract.Payment
{
    public interface IPaymentStrategyFactory
    {
        IPaymentStrategy GetStrategy(string paymentMethodName);
    }
}