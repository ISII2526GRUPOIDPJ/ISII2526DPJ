namespace AppForSEII2526.API.Models
{
    public class Bizum:PaymentMethod
    {
        public long TelephoneNumber { get; set; }

        [NotMapped]
        public new string Description { get { return TelephoneNumber.ToString(); } }
    }
}
