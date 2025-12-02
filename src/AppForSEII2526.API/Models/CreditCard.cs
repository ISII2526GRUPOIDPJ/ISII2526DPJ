namespace AppForSEII2526.API.Models
{
    public class CreditCard:PaymentMethod
    {
        public CreditCard()
        {
        }
        public CreditCard(int creditCardNumber, DateTime expirationDate, ApplicationUser user)
        {
            CreditCardNumber = creditCardNumber;
            ExpirationDate = expirationDate;
            User = user; // 'user' links this credit card to its owner (ApplicationUser).
        }


        public int CreditCardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }

        [NotMapped]
        public new string Description { get { return CreditCardNumber.ToString() + "" + ExpirationDate.ToString(); } }
    }
}
