using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaPub.Contract.Payment
{
    public interface IPaymentStrategy
    {
        string PaymentMethodName { get; } // Identifica o método de pagamento
        Task<bool> ProcessPayment(decimal value, int customerId);
    }
}