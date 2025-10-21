

namespace AppForSEII2526.API.DTOs.ClassDTOs
{
    public class ClassForPlanDTO
    {
        public ClassForPlanDTO(int id, string name, IList<string> typeItemNames, DateTime dateTime, decimal price)
        {
            Id = id;
            Name = name;
            TypeItemNames = typeItemNames;
            DateTime = dateTime;
            Price = price;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<string> TypeItemNames { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Price { get; set; }
    }
}
