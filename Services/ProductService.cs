using System.Threading.Tasks;
using ProvaPub.Contract;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Repository.Number;
using ProvaPub.Repository.Product;

namespace ProvaPub.Services
{
	public class ProductService : IProductServiceContract
	{
		private readonly IProductRepository _repository;
		public ProductService(IProductRepository repository)
		{
			_repository = repository;
		}

		public async Task<ProductList> ListProducts(int page)
		{
			var dataReturn = await  _repository.GetPaged(page, 10);

			return new ProductList() { HasNext = false, TotalCount = 10, Products = dataReturn.Items };
		}
	}
}
