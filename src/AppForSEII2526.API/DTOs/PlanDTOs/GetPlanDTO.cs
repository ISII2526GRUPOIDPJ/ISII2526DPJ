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

        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Weeks { get; set; }
        public string HealthIssues { get; set; }
        public IList<ClassInPlanDTO> Classes { get; set; }

    }
}
