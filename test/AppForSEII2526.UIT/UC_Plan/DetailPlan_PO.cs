using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class DetailPlan_PO : PageObject
    {

        By nameAndSurname = By.Id("NameSurname");
        By planName = By.Id("PlanName");
        By weeks = By.Id("Weeks");
        By healthIssues = By.Id("HealthIssues");
        By createdDate = By.Id("CreatedDate");
        By price = By.Id("TotalPrice");





        public DetailPlan_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public string GetUser()
        {
            return _driver.FindElement(nameAndSurname).Text;
        }
        public string GetPlanName()
        {
            return _driver.FindElement(planName).Text;
        }
        public string GetWeeks()
        {
            return _driver.FindElement(weeks).Text;
        }
        public string GetHealthIssues()
        {
            return _driver.FindElement(healthIssues).Text;
        }

        public string GetCreatedDate()
        {
            return _driver.FindElement(createdDate).Text;
        }

        /*
        public string GetClassJoined()
        {
            return _driver.FindElement(classJoined).Text.;
        }
        */
        
        public string GetPrice()
        {
            return _driver.FindElement(price).Text;
        }
    }
}
