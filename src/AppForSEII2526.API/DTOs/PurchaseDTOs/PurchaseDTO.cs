using AppForSEII2526.API.DTOs.PlanDTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseDTO
    {
        public PurchaseDTO(int id, string city, string country, string street, decimal totalPrice, string? description, PaymentMethodDTO paymentMethod, IList<PurchaseItemsDTO> purchaseItems)
        {
            Id = id;
            City = city;
            Country = country;
            Street = street;
            TotalPrice = totalPrice;
            Description = description;
            PaymentMethod = paymentMethod;
            PurchaseItems = purchaseItems;
        }

        public int Id { get; set; }
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
                   Id == dTO.Id &&
                   City == dTO.City &&
                   Country == dTO.Country &&
                   Street == dTO.Street &&
                   TotalPrice == dTO.TotalPrice &&
                   Description == dTO.Description &&
                   PaymentMethod.Equals(dTO.PaymentMethod) &&
                   PurchaseItems.SequenceEqual(dTO.PurchaseItems);
        }
    }
}
