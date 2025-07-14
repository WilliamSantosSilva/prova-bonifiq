using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProvaPub.Contract;
using ProvaPub.Contract.Payment;

namespace ProvaPub.Services.Strategy
{
    public class PaymentStrategyFactory : IPaymentStrategyFactory
    {
        private readonly IEnumerable<IPaymentStrategy> _paymentStrategies;

        public PaymentStrategyFactory(IEnumerable<IPaymentStrategy> paymentStrategies)
        {
            _paymentStrategies = paymentStrategies;
        }

        public IPaymentStrategy GetStrategy(string paymentMethodName)
        {
            var strategy = _paymentStrategies.FirstOrDefault(s =>
                s.PaymentMethodName.Equals(paymentMethodName, StringComparison.OrdinalIgnoreCase));

            if (strategy == null)
            {
                throw new ArgumentException($"Método de pagamento '{paymentMethodName}' não suportado.");
            }
            return strategy;
        }
    }
}