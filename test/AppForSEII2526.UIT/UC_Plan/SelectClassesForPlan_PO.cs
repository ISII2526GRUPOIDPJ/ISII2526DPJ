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

        public void SearchPlan(string type, string date = "")
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                wait.Until(driver =>
                {
                    try
                    {
                        var select = new SelectElement(driver.FindElement(inputType));
                        return select.Options.Count > 1; // Espera que tenga más opciones
                    }
                    catch
                    {
                        return false;
                    }
                });

                WaitForBeingClickable(inputType);

                // Seleccionar tipo
                if (string.IsNullOrEmpty(type))
                    type = "All"; 
                
                SelectElement selectElement = new SelectElement(_driver.FindElement(inputType));
                selectElement.SelectByText(type);

                // Date - como ella maneja fechas como string
                if (date != "")
                {
                    var dateInput = _driver.FindElement(inputDate);
                    dateInput.Clear();
                    dateInput.SendKeys(date); // Formato: "yyyy-MM-dd"
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
    }
}
