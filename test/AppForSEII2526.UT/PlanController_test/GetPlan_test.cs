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

            var allTests = new List<object[]>
            {
                new object[] { null, new List<GetPlanDTO> { expectedPlan } }, // Without date filter
                new object[] { today, new List<GetPlanDTO> { expectedPlan } }  // With date filter
            };

            return allTests;
        }

        // Test cases for no plans found
        public static IEnumerable<object[]> TestCasesFor_GetPlan_NotFound()
        {
            var today = DateTime.Today;

            var allTests = new List<object[]>
            {
                new object[] { today.AddYears(5)}, // Future date with no plans
                new object[] { today.AddDays(-10) }, // Past date with no plans
                new object[] { today.AddMonths(1) } // Another date with no plans
            };

            return allTests;
        }

        // Test that covers successful retrieval of plans
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetPlan_OK))]
        public async Task GetPlan_ReturnsOk_WithCorrectFiltering(DateTime? filterDate, List<GetPlanDTO> expectedPlans)
        {
            // Arrange
            var mock = new Mock<ILogger<PlanController>>();
            ILogger<PlanController> logger = mock.Object;
            var controller = new PlanController(_context, logger);

            // Act
            var result = await controller.GetPlan(filterDate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnPlans = Assert.IsType<List<GetPlanDTO>>(okResult.Value);
            
            Assert.Equal(expectedPlans, returnPlans);

            if (filterDate.HasValue)
            {
                Assert.All(returnPlans, p => Assert.Equal(filterDate.Value.Date, p.CreatedDate.Date));
            }
        }

        // Test that covers scenario when no plans are found
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetPlan_NotFound))]
        public async Task GetPlan_ReturnsNotFound_WhenNoPlansExist(DateTime? date)
        {
            // Arrange
            var mock = new Mock<ILogger<PlanController>>();
            ILogger<PlanController> logger = mock.Object;

            var controller = new PlanController(_context, logger);

            // Act
            var result = await controller.GetPlan(date);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
