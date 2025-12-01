using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.PurchaseController_test
{
    public class CreatePurchase_test : AppForSEII25264SqliteUT
    {
        public class CreditCard : PaymentMethod { }

        public CreatePurchase_test() {
            ApplicationUser user = new ApplicationUser(1, "John", "Doe");

            var paymentMethod = new CreditCard()
            {
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

            var purchase = new List<Purchase>() {
                new Purchase("Madrid", "Spain", "Main Street 123", DateTime.Parse("2024-01-10"), "Gym equipment", 150, purchaseItems, paymentMethod)
            };

            _context.AddRange(user);
            _context.AddRange(brands);
            _context.AddRange(types);
            _context.AddRange(items);
            _context.AddRange(paymentMethod);
            _context.AddRange(purchase);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreatePurchase_BadRequest()
        {
            var allTests = new List<object[]>
            {
                //Invalid payment method
                new object[] {
                    new CreatePurchaseDTO(
                        "Madrid",
                        "Spain",
                        "Main Street 123",
                        DateTime.Parse("2024-01-10"),
                        "Description",
                        0m,
                        4,
                        new List<PurchaseItemsDTO> {new PurchaseItemsDTO("Yoga Mat", "Nike", 10, 25m)},
                        null
                    ),
                    "Payment method is required"
                },

                //Zero quantity
                new object[] {
                    new CreatePurchaseDTO(
                        "Madrid",
                        "Spain",
                        "Main Street 123",
                        DateTime.Parse("2024-01-10"),
                        "Description",
                        0m,
                        0,
                        new List<PurchaseItemsDTO> {},
                        new PaymentMethodDTO(1, "CreditCard", "123456789 2025-12-31")
                    ),
                    "At least one item must be selected"
                },

                //No quantity available
                new object[] {
                    new CreatePurchaseDTO(
                        "Madrid",
                        "Spain",
                        "Main Street 123",
                        DateTime.Parse("2024-01-10"),
                        "Description",
                        0m,
                        11,
                        new List<PurchaseItemsDTO> {new PurchaseItemsDTO("Yoga Mat", "Nike", 11, 25m)},
                        new PaymentMethodDTO(1, "CreditCard", "123456789 2025-12-31")
                    ),
                    $"Error! There's no stock for 'Yoga Mat'."
                }
        };
            return allTests;
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_ReturnsOk_WhenValidData()
        {
            // Arrange
            var purchaseDto = new CreatePurchaseDTO(
                "Madrid",
                "Spain",
                "Main Street 123",
                DateTime.Parse("2024-01-10"),
                "Description",
                150m,
                4,
                new List<PurchaseItemsDTO> {new PurchaseItemsDTO("Yoga Mat", "Nike", 10, 25m)},
                new PaymentMethodDTO(1, "CreditCard", "123456789 2025-12-31")
            );

            var mock = new Mock<ILogger<PurchaseController>>();
            ILogger<PurchaseController> logger = mock.Object;

            var controller = new PurchaseController(_context, logger);

            // Act
            var result = await controller.CreatePurchase(purchaseDto);

            // Assert
            var okResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedPurchase = Assert.IsType<PurchaseDTO>(okResult.Value);
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreatePurchase_BadRequest))]
        public async Task CreatePurchase_ReturnsBadRequest_WhenInvalidData(CreatePurchaseDTO purchaseDto, string expectedMessage)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PurchaseController>>();
            var controller = new PurchaseController(_context, mockLogger.Object);

            // Act
            var result = await controller.CreatePurchase(purchaseDto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            var errorActual = details.Errors.First().Value[0];

            Assert.StartsWith(expectedMessage, errorActual);
        }
    }
}
