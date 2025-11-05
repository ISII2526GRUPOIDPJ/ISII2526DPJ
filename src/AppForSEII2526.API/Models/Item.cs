namespace AppForSEII2526.API.Models

{
    public class Item
    {
        public Item() { }
        public Item(string description, string name, decimal purchasePrice, int quantityAvailableForPurchase, int quantityForRestock, decimal? restockPrice, Brand brand)
        {
            Description = description;
            Name = name;
            PurchasePrice = purchasePrice;
            QuantityAvailableForPurchase = quantityAvailableForPurchase;
            QuantityForRestock = quantityForRestock;
            RestockPrice = restockPrice;
            Brand = brand;
        }

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
        public TypeItem TypeItem { get; set; }
        public Brand Brand { get; set; }
    }
}
