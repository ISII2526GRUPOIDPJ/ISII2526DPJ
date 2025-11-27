using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.PlanController_test
{
    public class CreatePlan_test : AppForSEII25264SqliteUT
    {
        public CreatePlan_test()
        {
            var user = new ApplicationUser
            {
                Name = "Antonio",
                Surname = "Garcia"
            };

            var typeItems = new List<TypeItem>
            {
                new TypeItem("Yoga"),
                new TypeItem("Cardio"),
                new TypeItem("Strength")
            };

            var today = DateTime.Today;
            var yogaClass = new Class("Morning Yoga", new List<TypeItem> { typeItems[0] }, today.AddDays(1).AddHours(9), 10.00m, 15);
            var cardioClass = new Class("Cardio Blast", new List<TypeItem> { typeItems[1] }, today.AddDays(2).AddHours(18), 12.50m, 20);
            var strengthClass = new Class("Strength Training", new List<TypeItem> { typeItems[2] }, today.AddDays(3).AddHours(17), 15.00m, 0);

            _context.Add(user);
            _context.SaveChanges(); 

            var creditCard = new CreditCard(12345678, DateTime.Today.AddYears(2), user);

            _context.AddRange(typeItems);
            _context.AddRange(new List<Class> { yogaClass, cardioClass, strengthClass });
            _context.Add(creditCard);
            _context.SaveChanges();
        }


        // Test that CreatePlan returns BadRequest for various invalid inputs
        public static IEnumerable<object[]> TestCasesFor_CreatePlan_BadRequest()
        {
            var allTests = new List<object[]>
            {
                // Alternative Flow 4: No classes selected
                new object[] {
                    new CreatePlanDTO(
                        null, // <- No classes selected
                        new List<PaymentMethodDTO> { new PaymentMethodDTO(1, "CreditCard") },
                        "Valid Plan",
                        "Description",
                        4,
                        "None",
                        1
                    ),
                    "At least one class must be selected."
                },

                // Alternative Flow 5: Invalid payment method
                new object[] {
                    new CreatePlanDTO(
                        new List<ClassInPlanDTO> {
                            new ClassInPlanDTO(1, "Morning Yoga", new List<string>{"Yoga"}, 10.00m, DateTime.Today.AddDays(1).AddHours(9), "Goal")
                        },
                        new List<PaymentMethodDTO> { new PaymentMethodDTO(1, "CreditCard") },
                        "Invalid Payment Plan",
                        "Description",
                        4,
                        "None",
                        -1 // <- Invalid ID
                    ),
                    "Selected payment method not found."
                },

                // Alternative Flow 5: Empty plan name
                new object[] {
                    new CreatePlanDTO(
                        new List<ClassInPlanDTO> {
                            new ClassInPlanDTO(1, "Morning Yoga", new List<string>{"Yoga"}, 10.00m, DateTime.Today.AddDays(1).AddHours(9), "Goal")
                        },
                        new List<PaymentMethodDTO> { new PaymentMethodDTO(1, "CreditCard") },
                        "",
                        "Description",
                        4,
                        "None",
                        1
                    ),
                    "Plan name is required"
                },

                // Alternative Flow 5: Invalid weeks (0)
                new object[] {
                    new CreatePlanDTO(
                        new List<ClassInPlanDTO> {
                            new ClassInPlanDTO(1, "Morning Yoga", new List<string>{"Yoga"}, 10.00m, DateTime.Today.AddDays(1).AddHours(9), "Goal")
                        },
                        new List<PaymentMethodDTO> { new PaymentMethodDTO(1, "CreditCard") },
                        "Invalid Weeks Plan",
                        "Description",
                        0, // <- Invalid weeks
                        "None",
                        1
                    ),
                    "Weeks must be between 1 and 52"
                },
                // Alternative Flow 5: Invalid weeks (greater than 52)
                new object[] {
                    new CreatePlanDTO(
                        new List<ClassInPlanDTO> {
                            new ClassInPlanDTO(1, "Morning Yoga", new List<string>{"Yoga"}, 10.00m, DateTime.Today.AddDays(1).AddHours(9), "Goal")
                        },
                        new List<PaymentMethodDTO> { new PaymentMethodDTO(1, "CreditCard") },
                        "Invalid Weeks Plan High",
                        "Description",
                        999, // <- Invalid weeks (too high)
                        "None",
                        1
                    ),
                    "Weeks must be between 1 and 52"
                },

                // Alternative Flow 7: Classes without capacity  
                new object[] {
                    new CreatePlanDTO(
                        new List<ClassInPlanDTO> {
                            new ClassInPlanDTO(3, "Strength Training", new List<string>{"Strength"}, 15.00m, DateTime.Today.AddDays(3).AddHours(17), "Goal")
                        },
                        new List<PaymentMethodDTO> { new PaymentMethodDTO(1, "CreditCard") },
                        "Plan No Capacity",
                        "Description",
                        4,
                        "None",
                        1
                    ),
                    "The following classes have no available capacity: Strength Training"
                }
        };

            return allTests;
        }


        // Test that CreatePlan returns Ok when valid plan data is provided
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePlan_ReturnsOk_WhenValidData()
        {
            // Arrange
            var selectedClasses = _context.Classes
                .Where(c => c.Capacity > 0) // Only classes with capacity
                .Take(2)
                .Select(c => new ClassInPlanDTO(
                    c.Id,
                    c.Name,
                    c.TypeItems.Select(ti => ti.Name).ToList(),
                    c.Price,
                    c.Date,
                    "Improve health" // Goal
                )).ToList();


            // Get all payment methods from the database
            var paymentMethods = _context.PaymentMethods.ToList();

            var planDto = new CreatePlanDTO(
                selectedClasses,
                paymentMethods.Select(pm => new PaymentMethodDTO(pm.Id, "Type")).ToList(),
                "Plan Fitness",
                "General wellness plan",
                4,
                "None",
                paymentMethods.First().Id
            );

            var mock = new Mock<ILogger<PlanController>>();
            ILogger<PlanController> logger = mock.Object;

            var controller = new PlanController(_context, logger);

            // Act
            var result = await controller.CreatePlan(planDto);

            // Assert
            var okResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedPlan = Assert.IsType<GetPlanDTO>(okResult.Value);
        }


        // Test that CreatePlan returns BadRequest for various invalid inputs
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreatePlan_BadRequest))]
        public async Task CreatePlan_ReturnsBadRequest_WhenInvalidData(CreatePlanDTO planDto, string expectedMessage)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PlanController>>();
            var controller = new PlanController(_context, mockLogger.Object);

            // Act
            var result = await controller.CreatePlan(planDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(expectedMessage, badRequestResult.Value);
        }
    }
}
