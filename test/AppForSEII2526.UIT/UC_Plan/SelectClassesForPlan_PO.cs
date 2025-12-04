using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class SelectClassesForPlan_PO : PageObject
    {
        By inputType = By.Id("typeSelect");
        By inputDate = By.Id("selectedDate");

        By buttonSearchPlan = By.Id("searchPlan");
        By tableClasses = By.Id("tableClassesForPlan");

        public SelectClassesForPlan_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchPlan(string type, DateTime? date = null) 
        {
            // Wait for the web element to be clickable
            WaitForBeingClickable(inputType);
            //_driver.FindElement(inputType).SendKeys(type);

            // Select the type from the dropdown
            if (type == "") type = "";
            SelectElement selectType = new SelectElement(_driver.FindElement(inputType));
            selectType.SelectByValue(type);


            //Date
            if (date.HasValue)
            {
                var dateString = date.Value.ToString("dd/MM/yyyy");
                var dateInput = _driver.FindElement(inputDate);
                dateInput.Clear();
                dateInput.SendKeys(dateString);
            }

            _driver.FindElement(buttonSearchPlan).Click();
        }

        public bool CheckListOfClasses(List<string[]> expectedClasses)
        {
            return CheckBodyTable(expectedClasses, tableClasses);
        }
    }
}
