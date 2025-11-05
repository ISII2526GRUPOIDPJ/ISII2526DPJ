using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppForSEII2526.API.DTOs.PlanDTOs;

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

        // Test that GetPlan returns all plans when no filter date is provided
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPlan_returns_all_plans()
        {
            var today = DateTime.Today;

            // Arrange
            var expectedPlans = new List<GetPlanDTO>
            {
                new GetPlanDTO(
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
                )
            };

            var mock = new Mock<ILogger<PlanController>>();
            ILogger<PlanController> logger = mock.Object;

            var controller = new PlanController(_context, logger);

            // Act
            var actionResult = await controller.GetPlan(null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnPlans = Assert.IsType<List<GetPlanDTO>>(okResult.Value);
            
            Assert.Equal(expectedPlans.Count, returnPlans.Count);
            for (int i = 0; i < expectedPlans.Count; i++)
            {
                Assert.Equal(expectedPlans[i], returnPlans[i]);
            }
        }

        // Test that GetPlan returns an empty list when no plans exist for the given future date
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPlan_no_plans_found_returns_NotFound()
        {
            // Arrange
            var futureDate = DateTime.Today.AddYears(5); // Using a future date with no plans

            var mock = new Mock<ILogger<PlanController>>();
            ILogger<PlanController> logger = mock.Object;
            var controller = new PlanController(_context, logger);

            // Act
            var actionResult = await controller.GetPlan(futureDate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var plans = Assert.IsType<List<GetPlanDTO>>(okResult.Value);
            Assert.Empty(plans); // There should be no plans
        }

        // Test that GetPlan returns only plans matching the specified date filter
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPlan_filter_by_date_returns_correct_plans()
        {
            // Arrange
            var today = DateTime.Today;
            var targetDate = today; // Date used in the filter (the one used for Antonio's plan)

            var mock = new Mock<ILogger<PlanController>>();
            ILogger<PlanController> logger = mock.Object;
            var controller = new PlanController(_context, logger);

            // Act
            var actionResult = await controller.GetPlan(targetDate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnPlans = Assert.IsType<List<GetPlanDTO>>(okResult.Value);

            // Use the DTO's property name CreatedDate
            Assert.All(returnPlans, p => Assert.Equal(targetDate.Date, p.CreatedDate.Date));
        }

    }
}
