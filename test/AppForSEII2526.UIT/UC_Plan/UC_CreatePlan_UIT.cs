using AppForMovies.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class UC_CreatePlan_UIT : UC_UIT
    {
        private SelectClassesForPlan_PO selectClassesForPlan_PO;
        private const string className1 = "Cardio Blast";
        private const string classType1 = "Cardio";
        private readonly DateTime classDate1 = DateTime.Today.AddDays(1).AddHours(7); // Today+1, 7:00
        private const string price1 = "10";

        private const string className2 = "Strength Training";
        private const string classType2 = "Strength";
        private readonly DateTime classDate2 = DateTime.Today.AddDays(2).AddHours(8); // Today+2, 8:00
        private const string price2 = "20";

        private const string className3 = "Yoga";
        private const string classType3 = "Yoga";
        private readonly DateTime classDate3 = DateTime.Today.AddDays(2).AddHours(9); // Today+2, 9:00
        private const string price3 = "12";


        private CreatePlan_PO createPlan_PO;

        public UC_CreatePlan_UIT(ITestOutputHelper output) : base(output)
        {
            selectClassesForPlan_PO = new SelectClassesForPlan_PO(_driver, _output);
            createPlan_PO = new CreatePlan_PO(_driver, _output);
        }

        /*
        private void Precondition_perform_login() {
            Perform_login("email","passwrod");
        }
        */

        private void InitialStepsForCreatingPlan()
        {
            //Precondition_perform_login();
            Initial_step_opening_the_web_page();
            selectClassesForPlan_PO.WaitForBeingVisible(By.Id("CreatePlan"));
            _driver.FindElement(By.Id("CreatePlan")).Click();
        }

        public void AddClassAndGoToCreatePlan(string className)
        {
            InitialStepsForCreatingPlan();

            selectClassesForPlan_PO.AddClassToPlan(className);

            Thread.Sleep(1500);

            // Go to the plan (click button at the right)
            selectClassesForPlan_PO.ClickGoToCreatePlan();

            // Wait until Create Plan UI is visible (by looking that the field Name appears)
            createPlan_PO.WaitForBeingVisible(By.Id("Name"));
        }

        // Use dbo.Classes.ForPlanning_AllAvailable.sql & dbo.TypeItems.ForPlanning_AllAvailable.sql to have the classes in the BD
        [Fact]
        [Trait("Level Testing", "Functional Testing")]
        public void UC31_3_AF1_FilterByType()
        {
            // Arrange
            InitialStepsForCreatingPlan();

            var expectedClasses = new List<string[]>
            {
                new string[] { className1, classType1, classDate1.ToString("dd/MM/yyyy HH:mm"), price1 }
            };

            // Act
            selectClassesForPlan_PO.SearchPlan(classType1);

            // Assert
            Assert.True(selectClassesForPlan_PO.CheckListOfClasses(expectedClasses));
        }

        // Use dbo.Classes.ForPlanning_AllAvailable.sql & dbo.TypeItems.ForPlanning_AllAvailable.sql to have the classes in the BD
        [Fact]
        [Trait("Level Testing", "Functional Testing")]
        public void UC31_4_AF1_FilterByDate()
        {
            // Arrange
            InitialStepsForCreatingPlan();

            var expectedClasses = new List<string[]>
            {
                new string[] { className1, classType1, classDate1.ToString("dd/MM/yyyy HH:mm"), price1 }
            };

            // Act
            string filterDate = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            selectClassesForPlan_PO.SearchPlan("", filterDate);

            // Assert
            Assert.True(selectClassesForPlan_PO.CheckListOfClasses(expectedClasses));
        }

        // Use dbo.Classes.ForPlanning_AllAvailable.sql & dbo.TypeItems.ForPlanning_AllAvailable.sql to have the classes in the BD
        [Fact]
        [Trait("Level Testing", "Functional Testing")]
        public void UC31_5_AF2_DateBeforeToday()
        {
            // Arrange
            InitialStepsForCreatingPlan();

            // Act
            string pastDate = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy");
            selectClassesForPlan_PO.SearchPlan("", pastDate);


            // Assert
            Assert.True(selectClassesForPlan_PO.CheckMessageError("Selected date must be today or later"), $"Error in the message box for test with date: {pastDate}");
        }


        [Theory]
        [InlineData("", "Description", "1", "Health Issue", "CreditCard", "The Name field is required.")]
        [InlineData("E", "Description", "1", "Health Issue", "CreditCard", "The field Name must be a string with a minimum length of 3 and a maximum length of 50")]
        [Trait("Level Testing", "Functional Testing")]
        public void UC31_8_9_AF5_ErrorInName(string name, string description, string weeks, string healthIssues, string paymentMethod, string expectedError)
        {
            // Arrange
            AddClassAndGoToCreatePlan(className1);

            // Act
            createPlan_PO.FillPlanForm(name, description, weeks, healthIssues, paymentMethod);
            createPlan_PO.ClickConfirmPlan();

            // Assert
            Assert.True(createPlan_PO.CheckMessageError(expectedError));
        }

        [Fact]
        [Trait("Level Testing", "Functional Testing")]
        public void UC31_10_AF5_ErrorInWeeks()
        {
            // Arrange
            AddClassAndGoToCreatePlan(className1);

            // Act
            createPlan_PO.FillPlanForm("Plan1", "Description", "53", "Health Issue", "CreditCard");
            createPlan_PO.ClickConfirmPlan();

            // Assert
            Assert.True(createPlan_PO.CheckMessageError("The field Weeks must be between 1 and 52."));
        }
    }
}