namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class PurchaseItemsDTO
    {
        public PurchaseItemsDTO(string name, string brand, int quantity)
        {
            Name = name;
            Brand = brand;
            Quantity = quantity;
        }

        public string Name { get; set; }
        public string Brand { get; set; }
        public int Quantity { get; set; }
    }
}
