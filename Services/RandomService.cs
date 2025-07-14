using Microsoft.EntityFrameworkCore;
using ProvaPub.Contract;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Repository.Number;

namespace ProvaPub.Services
{
	public class RandomService : IRandomServiceContract
	{
        private readonly INumberRepository _repository;
        private static readonly Random _random = new Random();
        
		public RandomService(INumberRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<int> GetRandom()
		{
            try
            {
                var number =  new Random(Guid.NewGuid().GetHashCode()).Next(100);

                var _numberEntity = new Entity.RandomNumber(number);

                await _repository.Add(_numberEntity);

                return number;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.ToString());
            }           
		}

	}
}
