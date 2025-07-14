using ProvaPub.Contract;
using ProvaPub.Contract.Base;
using ProvaPub.Contract.Payment;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Repository.Order;

namespace ProvaPub.Services
{
	public class OrderService : Base.ServiceBase<Entity.Order> ,IOrderServiceContract
	{
		private readonly IOrderRepository _repository;
		private readonly IPaymentStrategyFactory _paymentStrategyFactory;
		public OrderService(IOrderRepository repository, IPaymentStrategyFactory paymentStrategyFactory) : base(repository)
		{
			_repository = repository;
			_paymentStrategyFactory = paymentStrategyFactory;
		}

		public async Task<Models.OrderModel> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
		{
			var paymentStrategy = _paymentStrategyFactory.GetStrategy(paymentMethod);
			bool paymentSuccessful = await paymentStrategy.ProcessPayment(paymentValue, customerId);

			if (!paymentSuccessful)
			{
				throw new Exception("Falha ao processar pagamento."); // Ou uma exceção mais específica
			}

			var order = new Entity.Order
			{
				Value = paymentValue,
				CustomerId = customerId,
				OrderDate = DateTime.UtcNow // <-- SALVANDO COMO UTC AQUI!
			};

			await _repository.Add(order);

			order.SetDateBrazilCustomization();

			return new OrderModel
			{
				Value = order.Value,
				CustomerId = order.CustomerId,
				Id = order.Id,
				OrderDate = order.OrderDate
			};

		}
	}
}
