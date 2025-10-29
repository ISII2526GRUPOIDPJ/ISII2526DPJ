
using SQLitePCL;

namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForPurchaseDTO
    {
        public ItemForPurchaseDTO(string name, string brand, string description, decimal price, int quantity)
        {
            Name = name;
            Brand = brand;
            Description = description;
            Price = price;
            Quantity = quantity;
        }

        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
