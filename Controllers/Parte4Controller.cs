﻿using Microsoft.AspNetCore.Mvc;
using ProvaPub.Contract;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services;

namespace ProvaPub.Controllers
{
	
	/// <summary>
	/// O Código abaixo faz uma chmada para a regra de negócio que valida se um consumidor pode fazer uma compra.
	/// Crie o teste unitário para esse Service. Se necessário, faça as alterações no código para que seja possível realizar os testes.
	/// Tente criar a maior cobertura possível nos testes.
	/// 
	/// Utilize o framework de testes que desejar. 
	/// Crie o teste na pasta "Tests" da solution
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class Parte4Controller :  ControllerBase
	{
		private readonly ICustomerServiceContract _service;
		public Parte4Controller(ICustomerServiceContract service)
		{
			_service = service;
		}

        [HttpGet("CanPurchase")]
		public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
		{
			return await _service.CanPurchase(customerId, purchaseValue);
		}
	}
}
