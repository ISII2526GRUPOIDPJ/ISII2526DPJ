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

        By dialogOkButton = By.Id("Button_DialogOK");

        By capacityErrorsBy = By.Id("CapacityErrors");


        public CreatePlan_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillPlanForm(string name, string description, string weeks, string healthIssues, string paymentMethod = "CreditCard",
            List<(string className, string goal)>? classGoals = null)
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

            // Class Goals
            if (classGoals != null)
            {
                foreach (var (className, goal) in classGoals)
                {
                    var goalInput = _driver.FindElement(By.Id("GoalInput_" + className));
                    goalInput.Clear();
                    goalInput.SendKeys(goal);
                }
            }
        }

        public void ClickModifyClasses()
        {
            _driver.FindElement(buttonModifyClasses).Click();
        }

        public void ClickConfirmPlan()
        {
            var element = _driver.FindElement(buttonConfirmPlan);

            // Force scroll
            Actions actions = new Actions(_driver);
            actions.MoveToElement(element).Perform();

            element.Click();
        }


        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }

        public void ClickDialogOk()
        {
            WaitForBeingClickable(dialogOkButton);
            _driver.FindElement(dialogOkButton).Click();
        }

        public bool CheckCapacityError(string errorMessage)
        {
            try
            {
                IWebElement errorElement = _driver.FindElement(capacityErrorsBy);
                _output.WriteLine($"Capacity error: {errorElement.Text}");
                return errorElement.Text.Contains(errorMessage);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
