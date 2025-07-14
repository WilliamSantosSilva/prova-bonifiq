using ProvaPub.Extensions;

namespace ProvaPub.Entity
{
	public class Order : Base.BaseEntity
	{
		public decimal Value { get; set; }
		public int CustomerId { get; set; }
		public DateTime OrderDate { get; set; }
		public Customer Customer { get; set; }

		public void SetDateBrazilCustomization()
		{			
			this.OrderDate = OrderDate.CastTimeZone("America/Sao_Paulo");
		}
	}
}
