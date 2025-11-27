using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
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
        public class TestPaymentMethod : PaymentMethod { }

        public GetPurchase_test() {

            ApplicationUser user = new ApplicationUser(1, "John", "Doe");

            var paymentMethod = new TestPaymentMethod() {
                Id = 1,
                User = user
            };

            var purchase = new List<Purchase>() {
                new Purchase("Madrid", "Spain", DateTime.Parse("2024-01-10"), "Gym equipment", "Main Street 123", 150, paymentMethod)
            };

            _context.AddRange(user);
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

            var purchase = _context.Purchases
                .Include(p => p.PaymentMethod)
                .ThenInclude(pm => pm.User)
                .First();

            var expectedPurchase = new PurchaseDTO(
                "Madrid",
                "Spain",
                "Main Street 123",
                150,
                "Gym equipment",
                new PaymentMethodDTO(1, "CreditCard"),
                new List<PurchaseItemsDTO> {new PurchaseItemsDTO("Yoga Mat", "Nike", 10, 25m)}
            );

            //Act
            var result = await controller.GetPurchase(1);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var purchaseDTOActual = Assert.IsType<PurchaseDTO>(okResult.Value);
            Assert.Equal(expectedPurchase, purchaseDTOActual);
        }
    }
}