namespace AppForSEII2526.API.DTOs.PlanDTOs
{
    public class ClassInPlanDTO
    {
        public ClassInPlanDTO(string name, IList<string> typeItemNames, decimal price, DateTime date, string goal)
        {
            Name = name;
            TypeItemNames = typeItemNames;
            Price = price;
            Date = date;
            Goal = goal;
        }

        public string Name { get; set; }
        public IList<string> TypeItemNames { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public string Goal { get; set; }
    }
}
