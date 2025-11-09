namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseDTO
    {
        public PurchaseDTO(string city, string country, string street, decimal totalPrice, string description, PaymentMethod paymentMethod, IList<PurchaseItemsDTO> purchaseItems)
        {
            City = city;
            Country = country;
            Street = street;
            TotalPrice = totalPrice;
            Description = description;
            PaymentMethod = paymentMethod;
        }

        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
