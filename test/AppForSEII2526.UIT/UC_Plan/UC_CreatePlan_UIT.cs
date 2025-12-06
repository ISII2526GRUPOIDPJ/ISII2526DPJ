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

        /*
        private const string className2 = "Strength Training";
        private const string classType2 = "Strength";
        private readonly DateTime classDate2 = new DateTime(2025, 12, 06, 08, 00, 00);
        private const string price2 = "20.00";
        */

        public UC_CreatePlan_UIT(ITestOutputHelper output) : base(output)
        {
            selectClassesForPlan_PO = new SelectClassesForPlan_PO(_driver, _output);
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


    }
}


/*
 * Hacer script con quantity 0 y meterlo en UC_Plan e indicar el nombre
[Fact]
[Trait("LevelTesting", "Functional Testing")]
public void UC31_noquantity()
{
    //Arrange
    InitialStepsForCreatingPlan();
    var expectedClasses = new List<string[]>
    {
        new string[] { className2, classType2, classDate2.ToString("dd/MM/yyyy HH:mm"), price2 }
    };

    selectClassesForPlan_PO.SearchPlan("");
}
*/

/*
[Fact]
[InlineData(className1, classType1, classDate1.ToString("dd/MM/yyyy HH:mm"), price1, "Cardio")]
[Trait("Level Testing", "Functional Testing")]
public void UC31_AF1_3_4_5_filter(string className, string classType, string classDate, string classPrice,
    string filterType)
{
    // Arrange
    InitialStepsForCreatingPlan();
    var expectedClasses = new List<string[]>
    {
        new string[] { className, classType, classDate.ToString("dd/MM/yyyy HH:mm"), price }
    };

    // Act
    selectClassesForPlan_PO.SearchPlan(className);

    // Assert
    Assert.True(selectClassesForPlan_PO.CheckListOfClasses(expectedClasses));
}
*/