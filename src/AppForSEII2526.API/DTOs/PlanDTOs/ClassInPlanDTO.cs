namespace AppForSEII2526.API.DTOs.PlanDTOs
{
    public class ClassInPlanDTO
    {
        public ClassInPlanDTO(int id, string name, IList<string> typeItemNames, decimal price, DateTime date, string goal)
        {
            Id = id;
            Name = name;
            TypeItemNames = typeItemNames;
            Price = price;
            Date = date;
            Goal = goal;
        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Class name cannot be longer than 50 characters.")]
        [MinLength(3, ErrorMessage = "Class name must be at least 3 characters.")]
        public string Name { get; set; }
        public IList<string> TypeItemNames { get; set; }

        [Precision(10, 2)]
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        [StringLength(200, ErrorMessage = "Goal cannot be longer than 200 characters.")]
        public string? Goal { get; set; }
    }
}
