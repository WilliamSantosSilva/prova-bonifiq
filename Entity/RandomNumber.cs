namespace ProvaPub.Entity
{
    public class RandomNumber : Base.BaseEntity
    {
        public RandomNumber(int number)
        {
            Number = number;            
        }
        public int Number { get; set; }
    }
}
