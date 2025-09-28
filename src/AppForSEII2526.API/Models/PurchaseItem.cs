namespace AppForSEII2526.API.Models
{
    public class PurchaseItem
    {
        [Key]
        public int ItemId { get; set; }
        public int Amount_bought { get; set; }
        public decimal Price { get; set; }
        public int PurchaseId { get; set; }
    }
}
