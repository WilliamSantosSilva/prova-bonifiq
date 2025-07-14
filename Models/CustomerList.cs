namespace ProvaPub.Models
{
	public class CustomerList
	{
		public IEnumerable<Entity.Customer> Customers { get; set; }
		public int TotalCount { get; set; }
		public bool HasNext { get; set; }
	}
}
