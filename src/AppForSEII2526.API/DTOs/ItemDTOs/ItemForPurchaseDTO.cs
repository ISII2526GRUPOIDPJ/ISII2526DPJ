
using SQLitePCL;

namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForPurchaseDTO
    {
        public ItemForPurchaseDTO(string name, string brand, string description, IList<decimal> purchaseItems, int quantity)
        {
            Name = name;
            Brand = brand;
            Description = description;
            PurchaseItems = purchaseItems;
            Quantity = quantity;
        }

        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public IList<decimal> PurchaseItems { get; set; }
        public int Quantity { get; set; }

    }
}
