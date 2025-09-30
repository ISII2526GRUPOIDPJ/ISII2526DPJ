namespace AppForSEII2526.API.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        [Precision(10, 2)]
        public decimal PurchasePrice { get; set; }
        public int QuantityAvailableForPurchase { get; set; }
        public int QuantityForRestock { get; set; }
        [Precision(10, 2)]
        public decimal? RestockPrice { get; set; }
        public IList<PurchaseItem> PurchaseItems { get; set; }
    }
}
