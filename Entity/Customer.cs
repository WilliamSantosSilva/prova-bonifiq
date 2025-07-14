namespace ProvaPub.Entity
{
	public class Customer : Base.BaseEntity
	{
		public string Name { get; set; }
		public ICollection<Order> Orders { get; set; }
	}
}
