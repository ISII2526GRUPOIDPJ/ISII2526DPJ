namespace AppForSEII2526.API.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public string HealthIssues { get; set; }
        public string Name { get; set; }

        [Precision(10,2)]
        public decimal TotalPrice { get; set; }
        public int Weeks { get; set; }

        public IList<PlanItem> PlanItems { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
