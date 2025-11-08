namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Plan
    {
        public Plan()
        {
            PlanItems = new List<PlanItem>();
        }

        public Plan(DateTime createdDate, string name, string? description, string healthIssues, int weeks, decimal totalPrice, PaymentMethod paymentMethod, IList<PlanItem> planItems)
        {
            CreatedDate = createdDate;
            Name = name;
            Description = description;
            HealthIssues = healthIssues;
            Weeks = weeks;
            TotalPrice = totalPrice;
            PaymentMethod = paymentMethod;
            PlanItems = planItems ?? new List<PlanItem>();
        }

        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Description { get; set; }
        public string HealthIssues { get; set; }
        public string Name { get; set; }

        [Precision(10,2)]
        public decimal TotalPrice { get; set; }
        public int Weeks { get; set; }

        public IList<PlanItem> PlanItems { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
