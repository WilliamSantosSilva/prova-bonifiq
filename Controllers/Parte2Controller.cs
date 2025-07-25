﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProvaPub.Contract;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services;

namespace ProvaPub.Controllers
{
	
	[ApiController]
	[Route("[controller]")]
	public class Parte2Controller :  ControllerBase
	{
		private readonly IProductServiceContract _productServiceContract;
		private readonly ICustomerServiceContract _customerServiceContract;
		public Parte2Controller(IProductServiceContract productServiceContract,
				ICustomerServiceContract customerServiceContract)
		{
			_customerServiceContract = customerServiceContract;
			_productServiceContract = productServiceContract;
		}
		/// <summary>
		/// Precisamos fazer algumas alterações:
		/// 1 - Não importa qual page é informada, sempre são retornados os mesmos resultados. Faça a correção.
		/// 2 - Altere os códigos abaixo para evitar o uso de "new", como em "new ProductService()". Utilize a Injeção de Dependência para resolver esse problema
		/// 3 - Dê uma olhada nos arquivos /Models/CustomerList e /Models/ProductList. Veja que há uma estrutura que se repete. 
		/// Como você faria pra criar uma estrutura melhor, com menos repetição de código? E quanto ao CustomerService/ProductService. Você acha que seria possível evitar a repetição de código?
		/// 
		/// </summary>
		TestDbContext _ctx;
		public Parte2Controller(TestDbContext ctx)
		{
			_ctx = ctx;
		}
	
		[HttpGet("products")]
		public async Task<ProductList> ListProducts(int page)
		{
			return await _productServiceContract.ListProducts(page);
		}

		[HttpGet("customers")]
		public async Task<CustomerList> ListCustomers(int page)
		{
			return await _customerServiceContract.ListCustomers(page);
		}
	}
}
