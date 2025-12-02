using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UT.PlanController_test
{
    public class GetPlan_test : AppForSEII25264SqliteUT
    {
        public GetPlan_test()
        {
            var user = new ApplicationUser
            {
                Name = "Antonio",
                Surname = "Garcia"
            };

            var typeItems = new List<TypeItem>
            {
                new TypeItem("Yoga"),
                new TypeItem("Cardio")
            };

            var today = DateTime.Today;
            var yogaClass = new Class("Morning Yoga", new List<TypeItem> { typeItems[0] }, today.AddDays(1).AddHours(9), 10.00m, 15);
            var cardioClass = new Class("Cardio Blast", new List<TypeItem> { typeItems[1] }, today.AddDays(2).AddHours(18), 12.50m, 20);

            var planItems = new List<PlanItem>
            {
                new PlanItem(yogaClass, 10.00m, "Relaxation"),
                new PlanItem(cardioClass, 12.50m, "Stamina")
            };

            // Create payment method linked to the user
            var creditCard = new CreditCard(12345678, DateTime.Today.AddYears(2), user);

            var plan = new Plan(
                 today,
                 "Total Health Plan",
                 "General wellness plan",
                 "None",
                 4,
                 22.50m,
                 creditCard,
                 planItems
             );

            _context.Add(user);
            _context.AddRange(typeItems);
            _context.AddRange(new List<Class> { yogaClass, cardioClass });
            _context.Add(creditCard);
            _context.Add(plan);
            _context.AddRange(planItems);
            _context.SaveChanges();
        }

        // Test cases for successful retrieval
        public static IEnumerable<object[]> TestCasesFor_GetPlan_OK()
        {
            var today = DateTime.Today;

            var expectedPlan = new GetPlanDTO(
                1,
                "Antonio",
                "Garcia",
                today,
                22.50m,
                "Total Health Plan",
                "General wellness plan",
                4,
                "None",
                new List<ClassInPlanDTO>
                {
            new ClassInPlanDTO(1, "Morning Yoga", new List<string> { "Yoga" }, 10.00m, today.AddDays(1).AddHours(9), "Relaxation"),
            new ClassInPlanDTO(2, "Cardio Blast", new List<string> { "Cardio" }, 12.50m, today.AddDays(2).AddHours(18), "Stamina")
                }
            );

            return new List<object[]>
            {
                new object[] { 1, expectedPlan }
            };
        }

        // Test cases for no plans found
        public static IEnumerable<object[]> TestCasesFor_GetPlan_NotFound()
        {
            return new List<object[]>
            {
                new object[] { 999 }, // ID that does not exist
            };
        }

        // Test that covers successful retrieval of plans
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetPlan_OK))]
        public async Task GetPlan_ReturnsOk_WithCorrectFiltering(int planId, GetPlanDTO expectedPlan)
        {
            // Arrange
            var mock = new Mock<ILogger<PlanController>>();
            ILogger<PlanController> logger = mock.Object;
            var controller = new PlanController(_context, logger);

            // Act
            var result = await controller.GetPlan(planId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnPlan = Assert.IsType<GetPlanDTO>(okResult.Value);

            Assert.Equal(expectedPlan, returnPlan);
        }

        // Test that covers scenario when no plans are found
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetPlan_NotFound))]
        public async Task GetPlan_ReturnsNotFound_WhenNoPlansExist(int planId)
        {
            // Arrange
            var mock = new Mock<ILogger<PlanController>>();
            ILogger<PlanController> logger = mock.Object;
            var controller = new PlanController(_context, logger);

            // Act
            var result = await controller.GetPlan(planId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
