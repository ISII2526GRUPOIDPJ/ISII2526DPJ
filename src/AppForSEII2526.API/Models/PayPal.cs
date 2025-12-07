namespace AppForSEII2526.API.Models
{
    public class PayPal:PaymentMethod
    {
        [Required]
        public string Email { get; set; }

        [NotMapped]
        public new string Description { get { return Email; } }
    }
}
