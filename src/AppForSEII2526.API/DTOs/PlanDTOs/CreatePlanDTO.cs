using AppForSEII2526.API.DTOs.ClassDTOs;
using Humanizer;

namespace AppForSEII2526.API.DTOs.PlanDTOs
{
    public class CreatePlanDTO
    {
        public CreatePlanDTO(){ }

        public CreatePlanDTO(List<ClassInPlanDTO> selectedClasses, List<PaymentMethodDTO> availablePaymentMethods,
                             decimal totalPrice, string name, string description, int weeks, string healthIssues,
                             int selectedPaymentMethodId)
        {
            SelectedClasses = selectedClasses; // Uses ClassInPlanDTO so each selected class includes its own optional goal
            AvailablePaymentMethods = availablePaymentMethods;
            TotalPrice = totalPrice;
            Name = name;
            Description = description;
            Weeks = weeks;
            HealthIssues = healthIssues;
            SelectedPaymentMethodId = selectedPaymentMethodId;
        }

        public List<ClassInPlanDTO> SelectedClasses { get; set; }

        [Required(ErrorMessage = "Plan name is required")]
        [StringLength(50, ErrorMessage = "Plan name cannot be longer than 50 characters.")]
        [MinLength(3, ErrorMessage = "Plan name must be at least 3 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Number of weeks is required")]
        [Range(1, 52, ErrorMessage = "Weeks must be between 1 and 52")]
        public int Weeks { get; set; }

        [StringLength(200, ErrorMessage = "Health issues cannot be longer than 200 characters.")]
        public string? HealthIssues { get; set; }

        public List<PaymentMethodDTO> AvailablePaymentMethods { get; set; } 

        [Required(ErrorMessage = "Payment method is required")]
        public int SelectedPaymentMethodId { get; set; }

        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CreatePlanDTO dTO &&
                   ((SelectedClasses == null && dTO.SelectedClasses == null) ||
                    (SelectedClasses != null && dTO.SelectedClasses != null && SelectedClasses.SequenceEqual(dTO.SelectedClasses))) &&
                   Name == dTO.Name &&
                   Description == dTO.Description &&
                   Weeks == dTO.Weeks &&
                   HealthIssues == dTO.HealthIssues &&
                   ((AvailablePaymentMethods == null && dTO.AvailablePaymentMethods == null) ||
                    (AvailablePaymentMethods != null && dTO.AvailablePaymentMethods != null && AvailablePaymentMethods.SequenceEqual(dTO.AvailablePaymentMethods))) &&
                   SelectedPaymentMethodId == dTO.SelectedPaymentMethodId &&
                   TotalPrice == dTO.TotalPrice;
        }
    }
}
