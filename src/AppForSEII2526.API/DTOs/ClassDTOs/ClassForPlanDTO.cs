


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

        [StringLength(50, ErrorMessage = "Class name cannot be longer than 50 characters.")]
        [MinLength(3, ErrorMessage = "Class name must be at least 3 characters.")]
        public string Name { get; set; }
        public IList<string> TypeItemNames { get; set; }
        public DateTime DateTime { get; set; }

        [Precision(10, 2)]
        public decimal Price { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ClassForPlanDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   // Compare the actual list items so Assert.Equal works correctly
                   ((TypeItemNames == null && dTO.TypeItemNames == null) ||
                    (TypeItemNames != null && dTO.TypeItemNames != null && TypeItemNames.SequenceEqual(dTO.TypeItemNames))) &&
                   DateTime == dTO.DateTime &&
                   Price == dTO.Price;
        }
    }
}
