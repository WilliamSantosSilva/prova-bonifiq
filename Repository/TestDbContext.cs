using Bogus;
using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProvaPub.Repository
{

	public class TestDbContext : DbContext
	{
		public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Entity.Customer>().HasData(getCustomerSeed());
			modelBuilder.Entity<Entity.Product>().HasData(getProductSeed());

			modelBuilder.Entity<Entity.RandomNumber>().HasIndex(s => s.Number).IsUnique();
		}

		private Entity.Customer[] getCustomerSeed()
		{
			List<Entity.Customer> result = new();
			for (int i = 0; i < 20; i++)
			{
				result.Add(new Entity.Customer()
				{
					 Id = i+1,
					Name = new Faker().Person.FullName,
				});
			}
			return result.ToArray();
		}
		private Entity.Product[] getProductSeed()
		{
			List<Entity.Product> result = new();
			for (int i = 0; i < 20; i++)
			{
				result.Add(new Entity.Product()
				{
					Id = i + 1,
					Name = new Faker().Commerce.ProductName()
				});
			}
			return result.ToArray();
		}

		public DbSet<Entity.Customer> Customers{ get; set; }
		public DbSet<Entity.Product> Products{ get; set; }
		public DbSet<Entity.Order> Orders { get; set; }

        public DbSet<Entity.RandomNumber> Numbers { get; set; }

    }
}
