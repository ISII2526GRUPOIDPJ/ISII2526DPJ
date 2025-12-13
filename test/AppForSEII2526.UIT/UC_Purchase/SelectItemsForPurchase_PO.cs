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
        By itemBrand = By.Id("itemBrand");

        By buttonSearchItem = By.Id("SearchItem");
        By tableItems = By.Id("tableItemsForPurchase");

        By errorShownBy = By.Id("ErrorsShown");

        By buttonGoToCreatePurchase = By.Id("GoToCreatePurchaseButton");

        public SelectItemsForPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) {
        }

        public void SearchItems(string name, string brand) {
            WaitForBeingClickable(inputName);
            _driver.FindElement(inputName).SendKeys(name);

            if (string.IsNullOrEmpty(brand)) brand = "All";

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => new SelectElement(driver.FindElement(itemBrand)).Options.Count > 0);

            SelectElement selectElement = new SelectElement(_driver.FindElement(itemBrand));
            selectElement.SelectByText(brand);

            _driver.FindElement(buttonSearchItem).Click();
        }

        public bool CheckListOfItems(List<string[]> expectedItems) {
            return CheckBodyTable(expectedItems, tableItems);
        }

        public bool CheckMessageError(string errorMessage) {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"Actual message shown: {actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
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

        public bool IsCreatePurchaseButtonAvailable() {
            try {
                return _driver.FindElement(buttonGoToCreatePurchase).Displayed;
            } catch (NoSuchElementException) {
                return false;
            }
        }
    }
}
