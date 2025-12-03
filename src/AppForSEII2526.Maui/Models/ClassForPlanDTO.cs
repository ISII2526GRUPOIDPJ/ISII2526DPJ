// Models/DTOs/ClassForPlanDTO.cs
namespace AppForSEII2526.Maui.Models.DTOs
{
    public class ClassForPlanDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public IList<string> TypeItemNames { get; set; } = new List<string>();
        public DateTime DateTime { get; set; }
        public decimal Price { get; set; }
    }
}