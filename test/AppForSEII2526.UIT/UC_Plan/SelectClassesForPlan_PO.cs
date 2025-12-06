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

        By errorShownBy = By.Id("ErrorsShown");

        public SelectClassesForPlan_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchPlan(string type, string date = "")
        {
            try
            {
                WaitForBeingClickable(inputType);

                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                wait.Until(driver =>
                {
                    var select = new SelectElement(driver.FindElement(inputType));
                    return select.Options.Count > 1;
                });

                if (string.IsNullOrEmpty(type))
                    type = "All";

                var selectElement = new SelectElement(_driver.FindElement(inputType));
                selectElement.SelectByText(type);

                if (date != "")
                {
                    var dateInput = _driver.FindElement(inputDate);
                    dateInput.Clear();

                    // To transform it into DD/MM/YYYY
                    if (DateTime.TryParse(date, out DateTime dateValue))
                    {
                        dateInput.SendKeys(dateValue.ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        dateInput.SendKeys(date);
                    }
                }


                _driver.FindElement(buttonSearchPlan).Click();
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error en SearchPlan: {ex.Message}");
                throw;
            }
        }

        public bool CheckListOfClasses(List<string[]> expectedClasses)
        {
            return CheckBodyTable(expectedClasses, tableClasses);
        }

        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }
    }
}
