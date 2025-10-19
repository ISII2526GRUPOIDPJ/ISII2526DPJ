
namespace AppForSEII2526.API.DTOs.ClassDTOs
{
    public class ClassForPlanDTO
    {
        public ClassForPlanDTO(int id, string name, IList<string> typeItemNames, IList<string> planItemGoals)
        {
            Id = id;
            Name = name;
            TypeItemNames = typeItemNames;
            PlanItemGoals = planItemGoals;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<string> TypeItemNames { get; set; }
        public IList<string> PlanItemGoals { get; set; }
        }
}
