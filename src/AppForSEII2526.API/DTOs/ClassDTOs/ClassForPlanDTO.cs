namespace AppForSEII2526.API.DTOs.ClassDTOs
{
    public class ClassForPlanDTO
    {
        public ClassForPlanDTO(int id, string name, string goal)
        {
            Id = id;
            Name = name;
            Goal = goal;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public string Goal { get; set; }
    }
}
