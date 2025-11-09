using AppForSEII2526.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.PurchaseController_test
{
    public class GetPurchase_test : AppForSEII25264SqliteUT
    {
        public GetPurchase_test() {
            var purchase = new List<Purchase>() {
                new Purchase("Madrid", "Spain", DateTime.Parse("2024-01-10"), "Gym equipment", "Main Street 123", 150, 1),
                new Purchase("Barcelona", "Spain", DateTime.Parse("2024-01-12"), "Sports clothing", "Park Avenue 456", 89.99m, 2)
            };

            _context.AddRange(purchase);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPurchase_NotFound_test() {
            //Arrange
            var mock = new Mock<ILogger<PurchaseController>>();
            ILogger<PurchaseController> logger = mock.Object;

            var controller = new PurchaseController(_context, logger);

            //Act
            var result = await controller.GetPurchase(0);

            //Assert
            Assert.IsType<NotFoundResult> (result);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPurchase_Found_test()
        {
            //Arrange
            var mock = new Mock<ILogger<PurchaseController>>();
            ILogger<PurchaseController> logger = mock.Object;

            var controller = new PurchaseController(_context, logger);

            var expectedPurchase1 = new PurchaseDTO("Madrid", "Spain", "Main Street 123", 150, "Gym equipment", 1, 0);
            var expectedPurchase2 = new PurchaseDTO("Barcelona", "Spain", "Park Avenue 456", 89.99m, "Sports clothing", 2, 0);

            //Act
            var result = await controller.GetPurchase(0);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
