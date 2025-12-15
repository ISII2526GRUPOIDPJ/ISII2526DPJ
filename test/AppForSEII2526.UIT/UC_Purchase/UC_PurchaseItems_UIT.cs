using AppForMovies.UIT.Shared;
using AppForSEII2526.API.Models;
using AppForSEII2526.UIT.UC_Plan;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class UC_PurchaseItems_UIT : UC_UIT {
        private SelectItemsForPurchase_PO selectItemsForPurchase_PO;
        private CreatePurchase_PO createPurchase_PO;

        private const string itemName1 = "Yoga Mat";
        private const string itemBrand1 = "Nike";
        private const string itemDescription1 = "Yoga mat for exercises";
        private const string itemPrice1 = "25";
        private const string itemQuantity1 = "10";

        private const string itemName2 = "Running Shoes";
        private const string itemBrand2 = "Adidas";
        private const string itemDescription2 = "Running shoes";
        private const string itemPrice2 = "80";
        private const string itemQuantity2 = "15";

        public UC_PurchaseItems_UIT(ITestOutputHelper output) : base(output) {
            selectItemsForPurchase_PO = new SelectItemsForPurchase_PO(_driver, _output);
            createPurchase_PO = new CreatePurchase_PO(_driver, _output);
        }

        private void Precondition_perform_login() {
            Perform_login("test@uclm.es", "Test123!");
        }

        private void InitialStepsForCreatingPurchase() {
            Precondition_perform_login();
            //Initial_step_opening_the_web_page();

            selectItemsForPurchase_PO.WaitForBeingVisible(By.Id("CreatePurchase"));
            _driver.FindElement(By.Id("CreatePurchase")).Click();
        }

        public void AddItemAndGoToCreatePurchase(string itemName) {
            InitialStepsForCreatingPurchase();

            selectItemsForPurchase_PO.AddItemToPurchase(itemName);

            Thread.Sleep(1500);
            selectItemsForPurchase_PO.ClickGoToCreatePurchase();

            createPurchase_PO.WaitForBeingVisible(By.Id("City"));
        }

        public void AddItemAboveStockAndGoToCreatePurchase(string itemName, string quantityString) {
            InitialStepsForCreatingPurchase();

            selectItemsForPurchase_PO.AddItemToPurchase(itemName);

            selectItemsForPurchase_PO.WaitForBeingVisible(By.Id($"addItem_{itemName}"));
            int.TryParse(quantityString, out int quantity);
            for (int i = 0; i < quantity; i++) {
                _driver.FindElement(By.Id($"addItem_{itemName}")).Click();
            }

            Thread.Sleep(1500);
            selectItemsForPurchase_PO.ClickGoToCreatePurchase();

            createPurchase_PO.WaitForBeingVisible(By.Id("City"));
        }

        [Theory]
        [InlineData("Albacete", "Spain", "Main Street 123", "Gym equipment", 1, "123456789 2025-12-31")]
        [Trait("Level Testing", "Functional Testing")]
        public void UC45_1_BF_BasicFlow(string city,
            string country,
            string street,
            string description,
            int paymentMethod,
            string paymentMethodDescription) {
            // Arrange
            AddItemAndGoToCreatePurchase(itemName1);

            // Act
            createPurchase_PO.FillPurchaseForm(city, country, street, description, paymentMethod, paymentMethodDescription);
            createPurchase_PO.ClickConfirmPurchase();

            // Assert
            createPurchase_PO.ClickDialogOk();

            Thread.Sleep(2000);
            Assert.True(createPurchase_PO.CheckSuccessfulPurchase());
        }

        //[Fact]
        [Fact(Skip = "Requires empty database because it has conflicts with other tests that need data.")]
        [Trait("Level Testing", "Functional Testing")]
        public void UC45_2_AF1_NoItemsAvailableForPurchase() {
            // Arrange
            InitialStepsForCreatingPurchase();

            Thread.Sleep(1000);

            // Assert
            Assert.True(selectItemsForPurchase_PO.CheckMessageError("No items found for the selected criteria."));
        }

        [Fact]
        [Trait("Level Testing", "Functional Testing")]
        public void UC45_3_AF2_FilterByName() {
            // Arrange
            InitialStepsForCreatingPurchase();

            var expectedItems = new List<string[]> {
                new string[] { itemName1, itemBrand1, itemDescription1, itemPrice1, itemQuantity1 }
            };

            // Act
            Thread.Sleep(1000);
            selectItemsForPurchase_PO.SearchItems(itemName1, "");

            // Assert
            Thread.Sleep(1000);
            Assert.True(selectItemsForPurchase_PO.CheckListOfItems(expectedItems));
        }

        [Fact]
        [Trait("Level Testing", "Functional Testing")]
        public void UC45_4_AF2_FilterByBrand() {
            // Arrange
            InitialStepsForCreatingPurchase();

            var expectedItems = new List<string[]> {
                new string[] { itemName1, itemBrand1, itemDescription1, itemPrice1, itemQuantity1 }
            };

            // Act
            Thread.Sleep(1000);
            selectItemsForPurchase_PO.SearchItems("", itemBrand1);

            // Assert
            Thread.Sleep(1000);
            Assert.True(selectItemsForPurchase_PO.CheckListOfItems(expectedItems));
        }

        [Fact]
        [Trait("Level Testing", "Functional Testing")]
        public void UC45_5_AF3_ModifyPurchase() {
            // Arrange
            AddItemAndGoToCreatePurchase(itemName1);

            // Act
            createPurchase_PO.ClickModifyItems();

            // Assert
            selectItemsForPurchase_PO.WaitForBeingVisible(By.Id("inputName"));
            Assert.Contains("/purchase/selectitemsforpurchase", _driver.Url);
        }

        [Theory]
        [InlineData("", "Spain", "Main Street 123", "Gym equipment", 1, "123456789 2025-12-31", "(*) The City field is required.")]
        [Trait("Level Testing", "Functional Testing")]
        public void UC45_6_AF4_ErrorInCity(string city,
            string country,
            string street,
            string description,
            int paymentMethod,
            string paymentMethodDescription,
            string expectedError) {
            // Arrange
            AddItemAndGoToCreatePurchase(itemName1);

            // Act
            createPurchase_PO.FillPurchaseForm(city, country, street, description, paymentMethod, paymentMethodDescription);
            createPurchase_PO.ClickConfirmPurchase();
            createPurchase_PO.ClickDialogOk();

            // Assert
            Thread.Sleep(1000);
            Assert.True(createPurchase_PO.CheckMessageError(expectedError));
        }

        [Theory]
        [InlineData("Albacete", "", "Main Street 123", "Gym equipment", 1, "123456789 2025-12-31", "(*) The Country field is required.")]
        [Trait("Level Testing", "Functional Testing")]
        public void UC45_7_AF4_ErrorInCountry(string city,
            string country,
            string street,
            string description,
            int paymentMethod,
            string paymentMethodDescription,
            string expectedError) {
            // Arrange
            AddItemAndGoToCreatePurchase(itemName1);

            // Act
            createPurchase_PO.FillPurchaseForm(city, country, street, description, paymentMethod, paymentMethodDescription);
            createPurchase_PO.ClickConfirmPurchase();
            createPurchase_PO.ClickDialogOk();

            // Assert
            Thread.Sleep(1000);
            Assert.True(createPurchase_PO.CheckMessageError(expectedError));
        }

        [Theory]
        [InlineData("Albacete", "Spain", "", "Gym equipment", 1, "123456789 2025-12-31", "(*) The Street field is required.")]
        [Trait("Level Testing", "Functional Testing")]
        public void UC45_8_AF4_ErrorInStreet(string city,
            string country,
            string street,
            string description,
            int paymentMethod,
            string paymentMethodDescription,
            string expectedError) {
            // Arrange
            AddItemAndGoToCreatePurchase(itemName1);

            // Act
            createPurchase_PO.FillPurchaseForm(city, country, street, description, paymentMethod, paymentMethodDescription);
            createPurchase_PO.ClickConfirmPurchase();
            createPurchase_PO.ClickDialogOk();

            // Assert
            Thread.Sleep(1000);
            Assert.True(createPurchase_PO.CheckMessageError(expectedError));
        }

        [Theory]
        [InlineData("Albacete", "Spain", "Main Street 123", "Gym equipment", 0, "123456789 2025-12-31", "The selected payment method is not valid. Please select a valid payment method.")]
        [InlineData("Albacete", "Spain", "Main Street 123", "Gym equipment", 1, "", "The selected payment method is not valid. Please select a valid payment method.")]
        [Trait("Level Testing", "Functional Testing")]
        public void UC45_9_AF4_ErrorInPaymentMethod(string city,
            string country,
            string street,
            string description,
            int paymentMethod,
            string paymentMethodDescription,
            string expectedError) {
            // Arrange
            AddItemAndGoToCreatePurchase(itemName1);

            // Act
            createPurchase_PO.FillPurchaseForm(city, country, street, description, paymentMethod, paymentMethodDescription);
            createPurchase_PO.ClickConfirmPurchase();
            createPurchase_PO.ClickDialogOk();

            // Assert
            Thread.Sleep(2000);
            Assert.True(createPurchase_PO.CheckMessageError(expectedError));
        }

        [Theory]
        [InlineData("Albacete", "Spain", "Main Street 123", "Gym equipment", 1, "123456789 2025-12-31")]
        [Trait("Level Testing", "Functional Testing")]
        public void UC45_10_AF5_NoStock(string city,
            string country,
            string street,
            string description,
            int paymentMethod,
            string paymentMethodDescription) {
            // Arrange
            AddItemAboveStockAndGoToCreatePurchase(itemName1, itemQuantity1);

            // Act
            createPurchase_PO.FillPurchaseForm(city, country, street, description, paymentMethod, paymentMethodDescription);
            createPurchase_PO.ClickConfirmPurchase();

            Thread.Sleep(1000);
            createPurchase_PO.ClickDialogOk();

            // Assert
            Thread.Sleep(1000);
            Assert.True(createPurchase_PO.CheckPurchaseMessage($"(*) Error! There's no stock for '{itemName1}'."));
        }

        [Theory]
        [InlineData("Albacete", "Spain", "Main Street 123", "Gym equipment", 1, "123456789 2025-12-31")]
        [Trait("Level Testing", "Functional Testing")]
        public void UC45_11_BF_AF2_AF3(string city,
            string country,
            string street,
            string description,
            int paymentMethod,
            string paymentMethodDescription) {
            // Arrange
            InitialStepsForCreatingPurchase();

            var expectedItemsForFilter = new List<string[]> {
                new string[] { itemName2, itemBrand2, itemDescription2, itemPrice2, itemQuantity2 }
            };

            // Act
            //1. Add an item
            selectItemsForPurchase_PO.AddItemToPurchase(itemName1);

            //2. Filter by name
            selectItemsForPurchase_PO.SearchItems(itemName2, "");

            //3. Add a new item
            selectItemsForPurchase_PO.AddItemToPurchase(itemName2);

            //To include AF3
            selectItemsForPurchase_PO.ClickGoToCreatePurchase();
            createPurchase_PO.WaitForBeingVisible(By.Id("City"));

            createPurchase_PO.ClickModifyItems();
            selectItemsForPurchase_PO.WaitForBeingVisible(By.Id("inputName"));

            //4. Remove the first item
            selectItemsForPurchase_PO.RemoveItemFromPurchase(itemName1);

            //5. Create purchase
            selectItemsForPurchase_PO.ClickGoToCreatePurchase();
            createPurchase_PO.WaitForBeingVisible(By.Id("City"));

            createPurchase_PO.FillPurchaseForm(city, country, street, description, paymentMethod, paymentMethodDescription);
            createPurchase_PO.ClickConfirmPurchase();

            // Assert
            createPurchase_PO.ClickDialogOk();

            Thread.Sleep(2000);
            Assert.True(createPurchase_PO.CheckSuccessfulPurchase());
        }
    }
}
