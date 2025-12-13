using OpenQA.Selenium.Support.UI;
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
        By tableClasses = By.Id("tableClassesForPlan");

        By errorShownBy = By.Id("ErrorsShown");

        By buttonGoToCreatePlan = By.Id("GoToCreatePlanButton");

        public SelectClassesForPlan_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchPlan(string type, string date = "")
        {
            try
            {
                WaitForBeingClickable(inputType);

                // Needed for the dropdown to load its options
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                wait.Until(driver =>
                {
                    var select = new SelectElement(driver.FindElement(inputType));
                    return select.Options.Count > 0;
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

        public void ClickGoToCreatePlan()
        {
            // Wait until the button is clickable
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            var btn = wait.Until(
                ExpectedConditions.ElementToBeClickable(buttonGoToCreatePlan)
            );

            btn.Click();
        }

        public void AddClassToPlan(string className)
        {
            WaitForBeingClickable(By.Id("classToAdd_" + className));
            _driver.FindElement(By.Id("classToAdd_" + className)).Click();
        }

        public void RemoveClassFromPlan(string className)
        {
            WaitForBeingClickable(By.Id("removeClass_" + className));
            _driver.FindElement(By.Id("removeClass_" + className)).Click();
        }

        public bool IsCreatePlanButtonAvailable()
        {
            try
            {
                // If it is displayed = true, it is available
                return _driver.FindElement(buttonGoToCreatePlan).Displayed;
            }
            catch (NoSuchElementException)
            {
                // If it does not find the element = false, it is not available
                return false;
            }
        }
    }
}
