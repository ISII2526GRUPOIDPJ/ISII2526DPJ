namespace AppForSEII2526.API.Models
{
    public class PayPal:PaymentMethod
    {
        [Required]
        public string Email { get; set; }
    }
}
