namespace AppForSEII2526.API.Models
{
    public class Purchase
    {
        public Purchase() { }
        public Purchase(string city, string country, DateTime date, string description, string street, decimal total_price, PaymentMethod paymentMethod)
        {
            City = city;
            Country = country;
            Date = date;
            Description = description;
            Street = street;
            Total_price = total_price;
            PaymentMethod = paymentMethod;
        }

        public Purchase(string city, string country, string street, DateTime date, string? description, decimal total_price, IList<PurchaseItem> purchaseItems, PaymentMethod paymentMethod)
        {
            City = city;
            Country = country;
            Street = street;
            Date = date;
            Description = description;
            Total_price = total_price;
            PurchaseItems = purchaseItems;
            PaymentMethod = paymentMethod;
        }

        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        [Precision(10,2)]
        public decimal Total_price { get; set; }
        public IList<PurchaseItem> PurchaseItems { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}