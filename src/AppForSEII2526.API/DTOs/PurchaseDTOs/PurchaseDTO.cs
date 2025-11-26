using AppForSEII2526.API.DTOs.PlanDTOs;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseDTO
    {
        public PurchaseDTO(string city, string country, string street, decimal totalPrice, string? description, PaymentMethodDTO paymentMethod, IList<PurchaseItemsDTO> purchaseItems)
        {
            City = city;
            Country = country;
            Street = street;
            TotalPrice = totalPrice;
            Description = description;
            PaymentMethod = paymentMethod;
            PurchaseItems = purchaseItems;
        }

        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Description { get; set; }
        public PaymentMethodDTO PaymentMethod { get; set; }
        public IList<PurchaseItemsDTO> PurchaseItems { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseDTO dTO &&
                   City == dTO.City &&
                   Country == dTO.Country &&
                   Street == dTO.Street &&
                   TotalPrice == dTO.TotalPrice &&
                   Description == dTO.Description &&
                   EqualityComparer<PaymentMethodDTO>.Default.Equals(PaymentMethod, dTO.PaymentMethod) &&
                   EqualityComparer<IList<PurchaseItemsDTO>>.Default.Equals(PurchaseItems, dTO.PurchaseItems);
        }
    }
}
