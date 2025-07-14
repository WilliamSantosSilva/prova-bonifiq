using ProvaPub.Contract;
using ProvaPub.Contract.Payment;

namespace ProvaPub.Services.Payment
{
    public class CreditCardPaymentStrategy : IPaymentStrategy
    {
        public string PaymentMethodName => "creditcard";

        public async Task<bool> ProcessPayment(decimal value, int customerId)
        {
            Console.WriteLine($"Processando pagamento Credit Card de {value:C} para Cliente {customerId}");

            await Task.Delay(50);

            return true;
        }
    }
}