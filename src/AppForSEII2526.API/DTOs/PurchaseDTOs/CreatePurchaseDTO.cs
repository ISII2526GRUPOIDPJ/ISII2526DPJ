namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class CreatePurchaseDTO
    {
        public CreatePurchaseDTO() { }

        //Payment Method
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
    }
}
