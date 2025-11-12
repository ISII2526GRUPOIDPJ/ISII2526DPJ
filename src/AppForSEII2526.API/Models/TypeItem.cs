namespace AppForSEII2526.API.Models
{
    public class TypeItem
    {
        public TypeItem() { }
        public TypeItem(string name) {
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Item> Items { get; set; }
    }
}
