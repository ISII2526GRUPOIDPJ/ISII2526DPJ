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

        public override bool Equals(object? obj)
        {
            return obj is PurchaseItemsDTO dTO &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   Quantity == dTO.Quantity &&
                   Price == dTO.Price;
        }
    }
}
