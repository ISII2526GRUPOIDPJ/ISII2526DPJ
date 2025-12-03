using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

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

        public void SearchPlan(string type) 
        {
            // Wait for the web element to be clickable
            WaitForBeingClickable(inputType);
            //_driver.FindElement(inputType).SendKeys(type);

            // Select the type from the dropdown
            if (type == "") type = "All";
            SelectElement selectType = new SelectElement(_driver.FindElement(inputType));
            selectType.SelectByValue(type);
            

            _driver.FindElement(buttonSearchPlan).Click();
        }
    }
}
