namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class CreatePurchaseDTO
    {
        public CreatePurchaseDTO() { }
        public CreatePurchaseDTO(PaymentMethod paymentMethod, string city, string country, string street, string? description, int quantity)
        {
            PaymentMethod = paymentMethod;
            City = city;
            Country = country;
            Street = street;
            Description = description;
            Quantity = quantity;
        }
        public PaymentMethod PaymentMethod { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
    }
}
