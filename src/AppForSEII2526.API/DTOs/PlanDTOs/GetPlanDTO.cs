using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.PlanDTOs
{
    public class GetPlanDTO
    {
        public GetPlanDTO(string userName, string userSurname, DateTime createdDate, decimal totalPrice,
            string name, string description, int weeks, string healthIssues, IList<ClassInPlanDTO> classes)
        {
            UserName = userName;
            UserSurname = userSurname;
            CreatedDate = createdDate;
            TotalPrice = totalPrice;
            Name = name;
            Description = description;
            Weeks = weeks;
            HealthIssues = healthIssues;
            Classes = classes;
        }

        [StringLength(50, ErrorMessage = "User name cannot be longer than 50 characters.")]
        [MinLength(3, ErrorMessage = "User name must be at least 3 characters.")]
        public string UserName { get; set; }

        [StringLength(50, ErrorMessage = "User surname cannot be longer than 50 characters.")]
        [MinLength(3, ErrorMessage = "User surname must be at least 3 characters.")]
        public string UserSurname { get; set; }
        public DateTime CreatedDate { get; set; }

        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }

        [StringLength(50, ErrorMessage = "Plan name cannot be longer than 50 characters.")]
        [MinLength(3, ErrorMessage = "Plan name must be at least 3 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }
        public int Weeks { get; set; }

        [StringLength(200, ErrorMessage = "Health issues cannot be longer than 200 characters.")]
        public string HealthIssues { get; set; }
        public IList<ClassInPlanDTO> Classes { get; set; }

    }
}
