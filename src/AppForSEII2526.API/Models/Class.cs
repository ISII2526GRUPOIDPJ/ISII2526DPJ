namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), nameof(Date), IsUnique = true)]
    public class Class
    {
        public Class()
        {
            TypeItems = new List<TypeItem>();
            PlanItems = new List<PlanItem>();
        }


        public Class(string name, IList<TypeItem> typeItems, DateTime date, decimal price, int capacity)
        {
            Name = name;
            TypeItems = typeItems;
            Date = date;
            Price = price;
            Capacity = capacity; 
            PlanItems = new List<PlanItem>();
        }

        public int Id { get; set; }        
        public string Name { get; set; }
        public int Capacity { get; set; }
        public DateTime Date { get; set; }
        [Precision(10, 2)]
        public decimal Price { get; set; }

        public IList<PlanItem> PlanItems { get; set; }
        public IList<TypeItem> TypeItems { get; set; }
    }
}
