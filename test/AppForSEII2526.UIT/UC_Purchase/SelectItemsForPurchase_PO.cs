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
        protected SelectItemsForPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) {
        }

        public void SearchItems(string name, string brand) {
            WaitForBeingClickable(inputName);
            _driver.FindElement(inputName).SendKeys(name);
            //_driver.FindElement().Click();

            if (brand == "") brand = "All";
            SelectElement selectElement = new SelectElement(_driver.FindElement(inputBrand));
            selectElement.SelectByText(brand);
        }
    }
}
