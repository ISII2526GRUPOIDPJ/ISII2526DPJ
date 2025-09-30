using System.Numerics;

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(ItemId), nameof(PurchaseId))]
    public class PurchaseItem
    {
        public int ItemId { get; set; }
        public int Amount_bought { get; set; }
        [Precision(10,2)]
        public decimal Price { get; set; }
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
        public Item Item { get; set; }
    }
}
