namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class PurchaseDTO
    {
        public PurchaseDTO(Purchase purchase, string name, string brand, int quantity, IList<decimal> purchaseItems)
        {
            Purchase = purchase;
            Name = name;
            Brand = brand;
            Quantity = quantity;
            PurchaseItems = purchaseItems;
        }

        public Purchase Purchase { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public int Quantity { get; set; }
        public IList<decimal> PurchaseItems { get; set; }
    }
}
