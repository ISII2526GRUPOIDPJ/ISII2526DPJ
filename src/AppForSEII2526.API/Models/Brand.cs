namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Item> Items { get; set; }
    }
}
