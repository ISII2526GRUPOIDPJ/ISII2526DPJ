using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.Models;
using AppForSEII2526.UT;
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
        public class CreditCard : PaymentMethod { }

        public GetPurchase_test() {

            ApplicationUser user = new ApplicationUser("John", "Doe");

            var paymentMethod = new CreditCard() {
                Id = 1,
                User = user,
                Description = "123456789 2025-12-31"
            };

            var brands = new List<Brand>() {
                    new Brand("Nike"),
                    new Brand("Adidas")
            };

            var types = new List<TypeItem>() {
                    new TypeItem("Yoga"),
                    new TypeItem("Cardio"),
                    new TypeItem("Strenght")
            };

            var items = new List<Item> {
                new Item("Yoga mat for exercises", "Yoga Mat", 25.0m, 10, 5, 20, types[0], brands[0]),
                new Item("Running Shoes", "Running Shoes", 80, 15, 8, 70, types[1], brands[1]),
                new Item("Shirt for doing exercises", "Sports Shirt", 100, 0, 6, 85, types[2], brands[0])
            };

            var purchaseItems = new List<PurchaseItem> {
                new PurchaseItem(1, 1, items[0].PurchasePrice, items[0])
            };

            var purchase = new Purchase(
                "Madrid", "Spain", "Main Street 123", DateTime.Parse("2024-01-10"), "Gym equipment", 150, purchaseItems, paymentMethod
            );

            _context.AddRange(user);
            _context.AddRange(brands);
            _context.AddRange(types);
            _context.AddRange(items);
            _context.AddRange(paymentMethod);
            _context.AddRange(purchase);
            _context.AddRange(purchaseItems);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetPurchase_OK()
        {
            var expectedPurchase = new PurchaseDTO(
                1,
                "Madrid",
                "Spain",
                "Main Street 123",
                150,
                "Gym equipment",
                new PaymentMethodDTO(1, "CreditCard", "123456789 2025-12-31"),
                new List<PurchaseItemsDTO> { new PurchaseItemsDTO("Yoga Mat", "Nike", "Yoga mat for exercises", 1, 25.0m) }
            );

            return new List<object[]> {
                new object[] { 1, expectedPurchase }
            };
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

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetPurchase_OK))]
        public async Task GetPurchase_Found_test(int purchaseId, PurchaseDTO expectedPurchase)
        {
            //Arrange
            var mock = new Mock<ILogger<PurchaseController>>();
            ILogger<PurchaseController> logger = mock.Object;

            var controller = new PurchaseController(_context, logger);

            //Act
            var result = await controller.GetPurchase(purchaseId);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var purchaseDTOActual = Assert.IsType<PurchaseDTO>(okResult.Value);
            Assert.Equal(expectedPurchase, purchaseDTOActual);
        }
    }
}