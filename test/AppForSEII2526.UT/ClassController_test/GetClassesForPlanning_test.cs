using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppForSEII2526.API.DTOs.ClassDTOs;

namespace AppForSEII2526.UT.ClassController_test
{
    public class GetClassesForPlanning_test : AppForSEII25264SqliteUT
    {

        public GetClassesForPlanning_test()
        {
            var typeItems = new List<TypeItem>() {
                new TypeItem("Yoga"),
                new TypeItem("Cardio"),
                new TypeItem("Strength"),
                new TypeItem("Meditation"),
                new TypeItem("Dance"),
                new TypeItem("Martial Arts"),
            };

            var today = DateTime.Today;

            var classes = new List<Class>()
            {
                new Class("Strength Training", new List<TypeItem>{ typeItems[2] }, today.AddDays(1).AddHours(15), 20.00m, 4),
                new Class("Meditation", new List<TypeItem>{ typeItems[3] }, today.AddDays(2).AddHours(10), 8.00m, 7),
                new Class("Zumba", new List<TypeItem>{ typeItems[4] }, today.AddDays(3).AddHours(18).AddMinutes(30), 12.00m, 1),
                new Class("Kick-Boxing", new List<TypeItem>{ typeItems[5] }, today.AddDays(4).AddHours(17).AddMinutes(30), 19.00m, 10),
            };

            _context.AddRange(typeItems);
            _context.AddRange(classes);
            _context.SaveChanges();
        }

        // Test cases for successful retrieval
        public static IEnumerable<object[]> TestCasesFor_GetClassesForPlanning_OK()
        {
            var today = DateTime.Today;

            var classesDTOS = new List<ClassForPlanDTO>()
            {
                new ClassForPlanDTO(1, "Strength Training", new List<string>{"Strength"}, today.AddDays(1).AddHours(15), 20.00m),
                new ClassForPlanDTO(2, "Meditation", new List<string>{"Meditation"}, today.AddDays(2).AddHours(10), 8.00m),
                new ClassForPlanDTO(3, "Zumba", new List<string>{"Dance"}, today.AddDays(3).AddHours(18).AddMinutes(30), 12.00m),
                new ClassForPlanDTO(4, "Kick-Boxing", new List<string>{"Martial Arts"}, today.AddDays(4).AddHours(17).AddMinutes(30), 19.00m)
            };

            var allClasses = new List<ClassForPlanDTO>() {classesDTOS[0], classesDTOS[1], classesDTOS[2], classesDTOS[3]};
            var danceMartialArts = new List<ClassForPlanDTO>() { classesDTOS[2], classesDTOS[3] };
            var day2Classes = new List<ClassForPlanDTO>() { classesDTOS[1] };
            var day1Strength = new List<ClassForPlanDTO>() { classesDTOS[0] };


            var allTests = new List<object[]>
            {
                new object[] { null, null, allClasses},
                new object[] { today.AddDays(2), null, day2Classes },
                new object[] { today.AddDays(2), new string[] { }, day2Classes },
                new object[] { null, new string[] { "Dance", "Martial Arts" }, danceMartialArts },
                new object[] { today.AddDays(1), new string[] { "Strength" }, day1Strength }

            };

            return allTests;
        }

        // Test cases for error scenarios
        public static IEnumerable<object[]> TestCasesFor_GetClassesForPlanning_Error()
        {
            var today = DateTime.Today;

            var allTests = new List<object[]>
            {
                new object[] { today.AddDays(-1), null},                  // BadRequest
                new object[] { today.AddDays(10), null},                  // NotFound  
                new object[] { null, new string[] { "NonExistentType" }}, // NotFound  
                new object[] { today.AddDays(5), new string[] { "Yoga" }} // NotFound  
            };

            return allTests;
        }

        // Test that covers various filter combinations for date and types
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetClassesForPlanning_OK))]
        public async Task GetClassesForPlanning_filter_test(DateTime? date, string[]? types, List<ClassForPlanDTO> expectedClasses)
        {
            var today = DateTime.Today;

            // Arrange
            var mock = new Mock<ILogger<ClassController>>();
            ILogger<ClassController> logger = mock.Object;

            var controller = new ClassController(_context, logger);

            // Act
            var result = await controller.GetClassesForPlanning(date, types);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var classDTOsActual = Assert.IsType<List<ClassForPlanDTO>>(okResult.Value);

            Assert.Equal(expectedClasses, classDTOsActual);
        }


        // Test that covers various error scenarios for invalid parameters
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetClassesForPlanning_Error))]
        public async Task GetClassesForPlanning_ReturnsError_WithInvalidParameters( DateTime? date, string[]? types)
        {
            // Arrange
            var mock = new Mock<ILogger<ClassController>>();
            ILogger<ClassController> logger = mock.Object;
            var controller = new ClassController(_context, logger);

            // Act
            var result = await controller.GetClassesForPlanning(date, types);

            // Assert
            if (date.HasValue && date.Value.Date < DateTime.Today)
            {
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Cannot select classes from past dates.", badRequest.Value);
            }
            else
            {
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal("No classes found for the selected criteria.", notFound.Value);
            }
        }
    }
}