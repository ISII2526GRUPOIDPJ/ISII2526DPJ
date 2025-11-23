using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class PurchaseStateContainer
    {
        public CreatePurchaseDTO Purchase {  get; private set; } = new CreatePurchaseDTO() {
            PurchaseItems = new List<PurchaseItemsDTO>()
        };

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddItemToPurchase(PurchaseItemsDTO item)
        {
            
        }

        public void RemoveItemFromPurchase(PurchaseItemsDTO item)
        {
            Purchase.PurchaseItems.Remove(item);
        }

        public void ClearCart()
        {
            Purchase.PurchaseItems.Clear();
        }

        public void PurchaseProcessed()
        {
            Purchase = new CreatePurchaseDTO()
            {
                PurchaseItems = new List<PurchaseItemsDTO>()
            };
        }
    }
}
