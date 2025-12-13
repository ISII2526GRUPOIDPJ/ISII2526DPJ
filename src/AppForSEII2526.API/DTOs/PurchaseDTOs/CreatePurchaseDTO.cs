using System;
using AppForSEII2526.API.DTOs.PlanDTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class CreatePurchaseDTO
    {
        public CreatePurchaseDTO() { }

        public CreatePurchaseDTO(string city, string country, string street, DateTime date, string? description, decimal total_price, int quantity, IList<PurchaseItemsDTO> purchaseItems, PaymentMethodDTO paymentMethod)
        {
            City = city;
            Country = country;
            Street = street;
            Date = date;
            Description = description;
            Total_price = total_price;
            Quantity = quantity;
            PurchaseItems = purchaseItems;
            PaymentMethod = paymentMethod;
        }

        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Street { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        [Precision(10, 2)]
        public decimal Total_price { get; set; }
        public int Quantity { get; set; }
        public IList<PurchaseItemsDTO> PurchaseItems { get; set; }
        [Required]
        public PaymentMethodDTO PaymentMethod { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CreatePurchaseDTO dTO &&
                   City == dTO.City &&
                   Country == dTO.Country &&
                   Street == dTO.Street &&
                   Date == dTO.Date &&
                   Description == dTO.Description &&
                   Total_price == dTO.Total_price &&
                   Quantity == dTO.Quantity &&
                   Sequence.Equals(PurchaseItems, dTO.PurchaseItems) &&
                   PaymentMethod == dTO.PaymentMethod;
        }
    }
}
