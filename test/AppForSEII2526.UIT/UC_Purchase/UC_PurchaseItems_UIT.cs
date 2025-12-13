using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.UC_Plan;
using System;
using System.Collections.Generic;
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
            //Precondition_perform_login();
            Initial_step_opening_the_web_page();

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

        //[Fact]
        [Fact(Skip = "Requires empty database because it has conflicts with other tests that need data.")]
        [Trait("Level Testing", "Functional Testing")]
        public void UC45_2_AF1_NoItemsAvailableForPurchase() {
            // Arrange
            InitialStepsForCreatingPurchase();

            Thread.Sleep(1000);

            // Assert
            Assert.True(selectItemsForPurchase_PO.CheckMessageError("Error: No items found for the selected criteria."));
        }

        [Fact]
        [Trait("Level Testing", "Functional Testing")]
        public void UC45_3_AF2_FilterByName() {
            // Arrange
            InitialStepsForCreatingPurchase();

            var expectedItems = new List<string[]> {
                new string[] { itemName1, itemBrand1, itemDescription1, itemPrice1, itemQuantity1 }
            };

            Thread.Sleep(3000);

            // Act
            selectItemsForPurchase_PO.SearchItems(itemName1, "");

            // Assert
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

            Thread.Sleep(3000);

            // Act
            selectItemsForPurchase_PO.SearchItems("", itemBrand1);

            // Assert
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


    }
}
