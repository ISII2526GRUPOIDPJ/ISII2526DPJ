namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(PlanId), nameof(ClassId))]
    public class PlanItem
    {
        public PlanItem()
        {
        }
        public PlanItem(Class @class, decimal price, string goal)
        {
            Class = @class;
            Price = price;
            Goal = goal;
        }

        public int ClassId { get; set; }
        public string Goal { get; set; }
        public int PlanId { get; set; }
        [Precision(10, 2)]
        public decimal Price { get; set; }

        public Plan Plan { get; set; }
        public Class Class { get; set; }
    }
}
