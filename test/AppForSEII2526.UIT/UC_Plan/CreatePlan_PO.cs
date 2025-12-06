using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class CreatePlan_PO : PageObject
    {
        // Input Fields
        By inputName = By.Id("Name");
        By inputDescription = By.Id("Description");
        By inputWeeks = By.Id("Weeks");
        By inputHealthIssues = By.Id("HealthIssues");
        By selectPaymentMethod = By.Id("PaymentMethod");

        // Buttons
        By buttonModifyClasses = By.Id("ModifyClasses");
        By buttonConfirmPlan = By.Id("ConfirmPlan");

        // Table
        By tablePlanClasses = By.Id("TableOfPlanClasses");

        By errorShownBy = By.Id("ErrorsShown");


        public CreatePlan_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillPlanForm(string name, string description, string weeks, string healthIssues, string paymentMethod = "CreditCard")
        {

            // Name
            _driver.FindElement(inputName).Clear();
            _driver.FindElement(inputName).SendKeys(name);

            // Description
            _driver.FindElement(inputDescription).Clear();
            if (!string.IsNullOrEmpty(description))
            {
                _driver.FindElement(inputDescription).SendKeys(description);
            }

            // Weeks
            _driver.FindElement(inputWeeks).Clear();
            _driver.FindElement(inputWeeks).SendKeys(weeks);

            // Health Issues
            _driver.FindElement(inputHealthIssues).Clear();
            if (!string.IsNullOrEmpty(healthIssues))
            {
                _driver.FindElement(inputHealthIssues).SendKeys(healthIssues);
            }

            // Payment Method
            if (!string.IsNullOrEmpty(paymentMethod))
            {
                var selectElement = new SelectElement(_driver.FindElement(selectPaymentMethod));
                selectElement.SelectByText(paymentMethod);
            }
        }

        public void ClickModifyClasses()
        {
            _driver.FindElement(buttonModifyClasses).Click();
        }

        public void ClickConfirmPlan()
        {
            _driver.FindElement(buttonConfirmPlan).Click();
        }


        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }


    }
}
