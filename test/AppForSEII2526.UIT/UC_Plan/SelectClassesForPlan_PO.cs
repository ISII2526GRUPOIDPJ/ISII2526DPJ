using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class SelectClassesForPlan_PO : PageObject
    {
        By inputType = By.Id("typeSelect");
        By inputDate = By.Id("selectedDate");

        By buttonSearchPlan = By.Id("searchPlan");

        public SelectClassesForPlan_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchPlan(String type) 
        {
            // Wait for the web element to be clickable
            WaitForBeingClickable(inputType);
            _driver.FindElement(inputType).SendKeys(type);
            _driver.FindElement(buttonSearchPlan).Click();
        }
    }
}
