using ProvaPub.Contract;
using ProvaPub.Contract.Payment;

namespace ProvaPub.Services.Payment
{
    public class PixPaymentStrategy : IPaymentStrategy
    {
        public string PaymentMethodName => "pix";

        public async Task<bool> ProcessPayment(decimal value, int customerId)
        {
            Console.WriteLine($"Processando pagamento Pix de {value:C} para Cliente {customerId}");

            await Task.Delay(50);

            return true;
        }
    }
}