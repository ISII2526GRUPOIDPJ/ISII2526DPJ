using AppForMovies.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class UC_PurchaseItems_UIT : UC_UIT {
        private SelectItemsForPurchase_PO SelectItemsForPurchase_PO;

        private const string itemName1 = "Yoga Mat";
        private const string itemBrand1 = "Nike";
        private const string itemDescription1 = "Yoga mat for exercises";
        private const decimal itemPrice1 = 25m;
        private const int itemQuantity1 = 10;

        private const string itemName2 = "Running Shoes";
        private const string itemBrand2 = "Adidas";
        private const string itemDescription2 = "Running shoes";
        private const decimal itemPrice2 = 80m;
        private const int itemQuantity2 = 15;

        public UC_PurchaseItems_UIT(ITestOutputHelper output) : base(output) {

        }
    }
}
