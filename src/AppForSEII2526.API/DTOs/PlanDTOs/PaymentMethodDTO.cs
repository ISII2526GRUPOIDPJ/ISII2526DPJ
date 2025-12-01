namespace AppForSEII2526.API.DTOs.PlanDTOs
{
    public class PaymentMethodDTO
    {
        public PaymentMethodDTO() { }

        public PaymentMethodDTO(int id, string type, string description)
        {
            Id = id;
            Type = type;
            Description = description;
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; } 

        public override bool Equals(object? obj)
        {
            return obj is PaymentMethodDTO dTO &&
                   Id == dTO.Id &&
                   Type == dTO.Type &&
                   Description == dTO.Description;
        }
    }
}
