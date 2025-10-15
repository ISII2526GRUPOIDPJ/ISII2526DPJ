
using SQLitePCL;

namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForPurchaseDTO
    {
        public ItemForPurchaseDTO(int id, string name, string brand)
        {
            Id = id;
            Name = name;
            Brand = brand;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
    }
}
