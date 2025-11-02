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
                paymentMethods.Select(pm => new PaymentMethodDTO(pm.Id, "Type", "Info")).ToList(),
                selectedClasses.Sum(c => c.Price),
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
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPlan = Assert.IsType<GetPlanDTO>(okResult.Value);
        }

        // Test that CreatePlan returns BadRequest when the payment method is invalid or missing
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePlan_ReturnsBadRequest_WhenPaymentMethodInvalid()
        {
            // Arrange
            var selectedClasses = _context.Classes
                .Where(c => c.Capacity > 0)
                .Take(1)
                .Select(c => new ClassInPlanDTO(
                    c.Id,
                    c.Name,
                    c.TypeItems.Select(ti => ti.Name).ToList(),
                    c.Price,
                    c.Date,
                    "Improve health"
                )).ToList();

            // Setting an invalid payment method id
            var invalidPaymentMethodId = -1;

            var planDto = new CreatePlanDTO(
                selectedClasses,
                new List<PaymentMethodDTO>(), 
                selectedClasses.Sum(c => c.Price),
                "Invalid Payment Plan",
                "Testing invalid payment method",
                2,
                "None",
                invalidPaymentMethodId
            );

            var mockLogger = new Mock<ILogger<PlanController>>();
            var controller = new PlanController(_context, mockLogger.Object);

            // Act
            var result = await controller.CreatePlan(planDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Payment method is required.", badRequestResult.Value);
        }

        // Test that CreatePlan returns BadRequest when any selected class has zero capacity
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePlan_ReturnsBadRequest_WhenClassHasNoCapacity()
        {
            // Arrange: select only classes with zero capacity
            var selectedClasses = _context.Classes
                .Where(c => c.Capacity == 0) // Filter classes without capacity
                .Select(c => new ClassInPlanDTO(
                    c.Id,
                    c.Name,
                    c.TypeItems.Select(ti => ti.Name).ToList(),
                    c.Price,
                    c.Date,
                    "Goal"
                )).ToList();

            // Ensure there are classes without capacity; fail if empty
            Assert.NotEmpty(selectedClasses);

            var paymentMethods = _context.PaymentMethods.ToList();

            var planDto = new CreatePlanDTO(
                selectedClasses,
                paymentMethods.Select(pm => new PaymentMethodDTO(pm.Id, "Type", "Info")).ToList(),
                selectedClasses.Sum(c => c.Price),
                "Plan with No Capacity Classes",
                "This plan includes classes with zero capacity",
                3,
                "None",
                paymentMethods.First().Id
            );

            var mockLogger = new Mock<ILogger<PlanController>>();
            var controller = new PlanController(_context, mockLogger.Object);

            // Act
            var result = await controller.CreatePlan(planDto);

            // Assert
            var classesWithoutCapacity = selectedClasses.Select(c => c.Name).ToList();
            var expectedMessage = $"The following classes have no available capacity: {string.Join(", ", classesWithoutCapacity)}";

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(expectedMessage, badRequestResult.Value);
        }

        // Test that CreatePlan returns BadRequest when no classes are selected in the plan
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePlan_ReturnsBadRequest_WhenNoClassesSelected()
        {
            // Arrange
            var paymentMethods = _context.PaymentMethods.ToList();

            var planDto = new CreatePlanDTO(
                selectedClasses: null,  // No classes selected
                paymentMethods.Select(pm => new PaymentMethodDTO(pm.Id, "Type", "Info")).ToList(),
                totalPrice: 0m,
                name: "Plan without classes",
                description: "This plan has no classes selected",
                weeks: 4,
                healthIssues: "None",
                selectedPaymentMethodId: paymentMethods.First().Id
            );

            var mockLogger = new Mock<ILogger<PlanController>>();
            var controller = new PlanController(_context, mockLogger.Object);

            // Act
            var result = await controller.CreatePlan(planDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("At least one class must be selected.", badRequestResult.Value);
        }

        // Test that CreatePlan returns BadRequest when the plan name is empty or null
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePlan_ReturnsBadRequest_WhenPlanNameIsEmpty()
        {
            // Arrange
            var selectedClasses = _context.Classes
                .Where(c => c.Capacity > 0)
                .Take(1)
                .Select(c => new ClassInPlanDTO(
                    c.Id,
                    c.Name,
                    c.TypeItems.Select(ti => ti.Name).ToList(),
                    c.Price,
                    c.Date,
                    "Goal"
                )).ToList();

            var paymentMethods = _context.PaymentMethods.ToList();

            var planDto = new CreatePlanDTO(
                selectedClasses,
                paymentMethods.Select(pm => new PaymentMethodDTO(pm.Id, "Type", "Info")).ToList(),
                selectedClasses.Sum(c => c.Price),
                name: "",  // Empty plan name to trigger validation
                description: "Plan with empty name",
                weeks: 4,
                healthIssues: "None",
                selectedPaymentMethodId: paymentMethods.First().Id
            );

            var mockLogger = new Mock<ILogger<PlanController>>();
            var controller = new PlanController(_context, mockLogger.Object);

            // Act
            var result = await controller.CreatePlan(planDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Plan name is required", badRequestResult.Value);
        }

        // Test that CreatePlan returns BadRequest when the number of weeks is zero or less
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePlan_ReturnsBadRequest_WhenWeeksIsZeroOrLess()
        {
            // Arrange
            var selectedClasses = _context.Classes
                .Where(c => c.Capacity > 0)
                .Take(1)
                .Select(c => new ClassInPlanDTO(
                    c.Id,
                    c.Name,
                    c.TypeItems.Select(ti => ti.Name).ToList(),
                    c.Price,
                    c.Date,
                    "Improve health"
                )).ToList();

            var paymentMethods = _context.PaymentMethods.ToList();

            var planDto = new CreatePlanDTO(
                selectedClasses,
                paymentMethods.Select(pm => new PaymentMethodDTO(pm.Id, "Type", "Info")).ToList(),
                selectedClasses.Sum(c => c.Price),
                "Plan With Invalid Weeks",
                "Testing weeks <= 0",
                0,  // Invalid weeks
                "None",
                paymentMethods.First().Id
            );

            var mockLogger = new Mock<ILogger<PlanController>>();
            var controller = new PlanController(_context, mockLogger.Object);

            // Act
            var result = await controller.CreatePlan(planDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Number of weeks must be greater than 0.", badRequestResult.Value);
        }
    }
}
