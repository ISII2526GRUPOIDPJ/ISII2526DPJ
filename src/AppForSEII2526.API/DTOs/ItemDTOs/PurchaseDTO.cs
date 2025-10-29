namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class PurchaseDTO
    {
        public PurchaseDTO(string city, string country, string street, decimal totalPrice, string description, PaymentMethod paymentMethod, string name, string brand, int quantity)
        {
            City = city;
            Country = country;
            Street = street;
            TotalPrice = totalPrice;
            Description = description;
            PaymentMethod = paymentMethod;
            Name = name;
            Brand = brand;
            Quantity = quantity;
        }

        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public int Quantity { get; set; }
    }
}
