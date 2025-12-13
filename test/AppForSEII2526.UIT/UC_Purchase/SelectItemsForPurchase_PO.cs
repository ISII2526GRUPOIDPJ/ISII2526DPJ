using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class SelectItemsForPurchase_PO : PageObject {
    
        By inputName = By.Id("inputName");
        By inputBrand = By.Id("inputBrand");

        By buttonSearchItem = By.Id("searchItem");
        By tableItems = By.Id("tableItemsForPurchase");

        By errorShownBy = By.Id("ErrorsShown");

        By buttonGoToCreatePurchase = By.Id("GoToCreatePurchaseButton");

        protected SelectItemsForPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) {
        }

        public void SearchItems(string name, string brand) {
            WaitForBeingClickable(inputName);
            _driver.FindElement(inputName).SendKeys(name);

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => {
                var select = new SelectElement(driver.FindElement(inputBrand));
                return select.Options.Count > 0;
            });

            if (brand == "") brand = "All";
            SelectElement selectElement = new SelectElement(_driver.FindElement(inputBrand));
            selectElement.SelectByText(brand);

            _driver.FindElement(buttonSearchItem).Click();
        }

        public bool CheckListOfClasses(List<string[]> expectedItems) {
            return CheckBodyTable(expectedItems, tableItems);
        }

        public void ClickGoToCreatePurchase() {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            var btn = wait.Until(
                ExpectedConditions.ElementToBeClickable(buttonGoToCreatePurchase)
            );

            btn.Click();
        }

        public void AddItemToPurchase(string itemName) {
            WaitForBeingClickable(By.Id("itemToAdd_" + itemName));
            _driver.FindElement(By.Id("itemToAdd_" + itemName)).Click();
        }

        public void RemoveItemFromPurchase(string itemName) {
            WaitForBeingClickable(By.Id("removeItem_" + itemName));
            _driver.FindElement(By.Id("removeItem_" + itemName)).Click();
        }
    }
}
