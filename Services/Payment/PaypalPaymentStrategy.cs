using ProvaPub.Contract;
using ProvaPub.Contract.Payment;

namespace ProvaPub.Services.Payment
{
    public class PaypalPaymentStrategy : IPaymentStrategy
    {
        public string PaymentMethodName => "paypal";

        public async Task<bool> ProcessPayment(decimal value, int customerId)
        {
            Console.WriteLine($"Processando pagamento Paypal de {value:C} para Cliente {customerId}");

            await Task.Delay(50);

            return true;
        }
    }
}