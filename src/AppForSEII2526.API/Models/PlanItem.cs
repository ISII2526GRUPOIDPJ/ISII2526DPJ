namespace AppForSEII2526.API.Models
{
    public class PlanItem
    {
        [Key]
        public int ClassId { get; set; }
        public string Goal { get; set; }
        public int PlanId { get; set; }
        public decimal Price { get; set; }

        public Plan Plan { get; set; }
    }
}
