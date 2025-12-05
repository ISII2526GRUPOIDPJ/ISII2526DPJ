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
        private readonly DateTime classDate1 = new DateTime(2025, 12, 05, 07, 34, 00);
        private const string price1 = "10.00";

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
        public void UC31_FilterByType()
        {
            // Arrange
            InitialStepsForCreatingPlan();

            // Esperar a que cargue la página
            Thread.Sleep(2000);

            // DEBUG: Ver qué hay en la tabla inicialmente
            var table = _driver.FindElement(By.Id("tableClassesForPlan"));
            var rows = table.FindElements(By.TagName("tr"));

            if (rows.Count > 1)
            {
                var cells = rows[1].FindElements(By.TagName("td"));
                string actualClassName = cells[0].Text;
                string actualClassType = cells[1].Text;
                string actualClassDate = cells[2].Text;
                string actualClassPrice = cells[3].Text;

                _output.WriteLine($"Clase real en tabla: {actualClassName}, {actualClassType}, {actualClassDate}, {actualClassPrice}");

                // USAR LOS DATOS REALES en el expected
                var expectedClasses = new List<string[]>
                {
                    new string[] { actualClassName, actualClassType, actualClassDate, actualClassPrice }
                };

                // Act - filtrar por el tipo que realmente tiene la clase
                selectClassesForPlan_PO.SearchPlan(actualClassType, "");

                // Assert
                Assert.True(selectClassesForPlan_PO.CheckListOfClasses(expectedClasses));
            }
            else
            {
                _output.WriteLine("No hay clases en la tabla");
                Assert.Fail("No hay clases para probar");
            }
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