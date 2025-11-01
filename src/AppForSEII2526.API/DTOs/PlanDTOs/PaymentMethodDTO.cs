namespace AppForSEII2526.API.DTOs.PlanDTOs
{
    public class PaymentMethodDTO
    {
        public PaymentMethodDTO() { }

        public PaymentMethodDTO(int id, string type, string displayInfo)
        {
            Id = id;
            Type = type;
            DisplayInfo = displayInfo;
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string DisplayInfo { get; set; }
    }
}
