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
            var brands = new List<Brand>() {
                    new Brand("Nike"),
                    new Brand("Adidas")
            };

            var types = new List<TypeItem>() {
                    new TypeItem("Yoga"),
                    new TypeItem("Cardio"),
                    new TypeItem("Strenght")
            };

            var items = new List<Item>() {
                    new Item("Yoga mat for exercises", "Yoga Mat", 25, 10, 5, 20, types[0], brands[0]),
                    new Item("Running Shoes", "Running Shoes", 80, 15, 8, 70, types[1], brands[1]),
                    new Item("Shirt for doing exercises", "Sports Shirt", 100, 0, 6, 85, types[2], brands[0])
            };

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

            var expectedPurchase1 = new PurchaseDTO("Madrid", "Spain", "Main Street 123", 150, "Gym equipment", 1, new PurchaseItemsDTO("Yoga Mat", "Nike", 10, 25);
            var expectedPurchase2 = new PurchaseDTO("Barcelona", "Spain", "Park Avenue 456", 89.99m, "Sports clothing", 2, new PurchaseItemsDTO("Running Shoes", "Adidas", 15, 80);

            //Act
            var result = await controller.GetPurchase(0);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
