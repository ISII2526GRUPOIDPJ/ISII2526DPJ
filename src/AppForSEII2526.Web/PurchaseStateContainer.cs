using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class PurchaseStateContainer
    {
        public CreatePurchaseDTO Purchase {  get; private set; } = new CreatePurchaseDTO() {
            PurchaseItems = new List<PurchaseItemsDTO>()
        };

        public decimal EstimatedTotalPrice {
            get {
                return Purchase.PurchaseItems.Sum(pi => Convert.ToDecimal(pi.Price));
            }
        }

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddItemToPurchase(PurchaseItemsDTO item) {
            var existing = Purchase.PurchaseItems.FirstOrDefault(i => i.Name == item.Name);
            if (existing != null) {
                existing.Quantity += item.Quantity;
            } else {
                Purchase.PurchaseItems.Add(item);
            }

            NotifyStateChanged();
        }

        public void RemoveItemFromPurchase(PurchaseItemsDTO item) {
            Purchase.PurchaseItems.Remove(item);

            NotifyStateChanged();
        }

        public void ClearCart() {
            Purchase.PurchaseItems.Clear();

            NotifyStateChanged();
        }

        public void PurchaseProcessed() {
            Purchase = new CreatePurchaseDTO()
            {
                PurchaseItems = new List<PurchaseItemsDTO>()
            };

            NotifyStateChanged();
        }
    }
}
