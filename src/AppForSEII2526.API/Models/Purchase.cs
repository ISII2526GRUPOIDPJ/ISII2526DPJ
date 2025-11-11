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
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        [Precision(10,2)]
        public decimal Total_price { get; set; }
        public IList<PurchaseItem> PurchaseItems { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}