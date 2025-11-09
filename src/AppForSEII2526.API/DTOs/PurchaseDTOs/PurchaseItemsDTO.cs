namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseItemsDTO
    {
        public PurchaseItemsDTO(string name, string brand, int quantity, decimal Price)
        {
            Name = name;
            Brand = brand;
            Quantity = quantity;
        }

        public string Name { get; set; }
        public string Brand { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
