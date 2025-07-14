using System.Linq.Expressions;
using Moq;
using ProvaPub.Contract;
using ProvaPub.Repository;
using ProvaPub.Repository.Customer;
using ProvaPub.Services;
using Xunit;

namespace ProvaPub.Tests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IOrderServiceContract> _mockOrderService;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockOrderService = new Mock<IOrderServiceContract>();
            _service = new CustomerService(_mockCustomerRepository.Object, _mockOrderService.Object);

        }

        // --- Testes para Validações de Entrada ---
        [Fact]
        public async Task CanMakePurchase_ShouldThrowArgumentOutOfRangeException_WhenCustomerIdIsZero()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => _service.CanPurchase(0, 100)
            );
        }

        [Fact]
        public async Task CanMakePurchase_ShouldThrowArgumentOutOfRangeException_WhenPurchaseValueIsZero()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => _service.CanPurchase(1, 0)
            );
        }

        // --- Testes para Regra de Negócio: Cliente Não Cadastrado ---
        [Fact]
        public async Task CanMakePurchase_ShouldThrowInvalidOperationException_WhenCustomerDoesNotExist()
        {
            // Arrange
            int nonExistentCustomerId = 999;
            _mockCustomerRepository.Setup(r => r.GetById(nonExistentCustomerId))
                .ReturnsAsync((Entity.Customer)null); // Simula que o cliente não existe

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CanPurchase(nonExistentCustomerId, 50)
            );

            _mockCustomerRepository.Verify(r => r.GetById(nonExistentCustomerId), Times.Once); // Verifica que o mock foi chamado
        }

        // --- Testes para Regra de Negócio: Uma compra por mês ---
        [Fact]
        public async Task CanMakePurchase_ShouldReturnFalse_WhenAlreadyPurchasedThisMonth()
        {
            // Arrange
            int customerId = 1;
            decimal purchaseValue = 200;
            var customer = new Entity.Customer { Id = customerId, Name = "Test Customer" };

            _mockCustomerRepository.Setup(r => r.GetById(customerId))
                .ReturnsAsync(customer);

            // Simula que o cliente já tem 1 pedido este mês
            _mockOrderService.Setup(s => s.GetCount(It.IsAny<Expression<Func<Entity.Order, bool>>>()))
                .ReturnsAsync(1);

            // Act
            var result = await _service.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.False(result);
            _mockCustomerRepository.Verify(r => r.GetById(customerId), Times.Once);
            _mockOrderService.Verify(s => s.GetCount(It.IsAny<Expression<Func<Entity.Order, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task CanMakePurchase_ShouldReturnTrue_WhenNoPurchaseThisMonth()
        {
            // Arrange
            int customerId = 1;
            decimal purchaseValue = 200;
            var customer = new Entity.Customer { Id = customerId, Name = "Test Customer" };

            _mockCustomerRepository.Setup(r => r.GetById(customerId))
                .ReturnsAsync(customer);

            // Simula que o cliente não tem pedidos este mês
            _mockOrderService.Setup(s => s.GetCount(It.IsAny<Expression<Func<Entity.Order, bool>>>()))
                .ReturnsAsync(0); // Zero pedidos este mês

            // Simula que o cliente já comprou antes (para passar a regra de 100,00)
            _mockCustomerRepository.Setup(r => r.GetCount(It.IsAny<Expression<Func<Entity.Customer, bool>>>()))
                .ReturnsAsync(1); // Já comprou antes

            // Act
            var result = await _service.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.True(result);
        }

        // --- Testes para Regra de Negócio: Primeira compra máx. 100,00 ---
        [Fact]
        public async Task CanMakePurchase_ShouldReturnFalse_WhenFirstPurchaseExceeds100()
        {
            // Arrange
            int customerId = 1;
            decimal purchaseValue = 101; // Valor acima do limite
            var customer = new Entity.Customer { Id = customerId, Name = "New Customer" };

            _mockCustomerRepository.Setup(r => r.GetById(customerId))
                .ReturnsAsync(customer);

            // Simula que é a primeira compra (zero pedidos anteriores)
            _mockCustomerRepository.Setup(r => r.GetCount(It.IsAny<Expression<Func<Entity.Customer, bool>>>()))
                .ReturnsAsync(0);

            // Simula que não há pedidos este mês
            _mockOrderService.Setup(s => s.GetCount(It.IsAny<Expression<Func<Entity.Order, bool>>>()))
                .ReturnsAsync(0);

            // Act
            var result = await _service.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.False(result);
            _mockCustomerRepository.Verify(r => r.GetCount(It.IsAny<Expression<Func<Entity.Customer, bool>>>()), Times.Once); // Verifica que o GetCount de cliente foi chamado
        }

        [Fact]
        public async Task CanMakePurchase_ShouldReturnTrue_WhenFirstPurchaseIsWithin100()
        {
            // Arrange
            int customerId = 1;
            decimal purchaseValue = 99; // Valor dentro do limite
            var customer = new Entity.Customer { Id = customerId, Name = "New Customer" };

            _mockCustomerRepository.Setup(r => r.GetById(customerId))
                .ReturnsAsync(customer);

            // Simula que é a primeira compra
            _mockCustomerRepository.Setup(r => r.GetCount(It.IsAny<Expression<Func<Entity.Customer, bool>>>()))
                .ReturnsAsync(0);

            // Simula que não há pedidos este mês
            _mockOrderService.Setup(s => s.GetCount(It.IsAny<Expression<Func<Entity.Order, bool>>>()))
                .ReturnsAsync(0);

            // Act
            var result = await _service.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.True(result);
        }
       

        // --- Cenário Completo de Sucesso ---
        [Fact]
        public async Task CanMakePurchase_ShouldReturnTrue_WhenAllConditionsMet()
        {
            // Arrange
            int customerId = 1;
            decimal purchaseValue = 75; // Menor que 100, mas vamos simular que já comprou antes
            var customer = new Entity.Customer { Id = customerId, Name = "Existing Customer" };

            _mockCustomerRepository.Setup(r => r.GetById(customerId))
                .ReturnsAsync(customer);

            // Simula que não há pedidos este mês
            _mockOrderService.Setup(s => s.GetCount(It.IsAny<Expression<Func<Entity.Order, bool>>>()))
                .ReturnsAsync(0);

            // Simula que o cliente já comprou antes
            _mockCustomerRepository.Setup(r => r.GetCount(It.IsAny<Expression<Func<Entity.Customer, bool>>>()))
                .ReturnsAsync(1); // Já comprou antes

            // Act
            var result = await _service.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.True(result);
        }
    }
}
