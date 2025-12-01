namespace AppForSEII2526.API.Models
{
    public abstract class PaymentMethod
    {
        public int Id { get; set; }
        public IList<Plan> Plans { get; set; }
        public IList<Purchase> Purchases { get; set; }
        public ApplicationUser User { get; set; }

        [NotMapped]
        public string? Description { get; set; }
    }
}
