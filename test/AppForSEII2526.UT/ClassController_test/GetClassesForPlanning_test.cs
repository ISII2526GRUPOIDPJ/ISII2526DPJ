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

        // Test that the endpoint returns all classes when no filters are applied
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetClassesForPlanning_null_date_type()
        {
            var today = DateTime.Today;

            // Arrange
            IList<ClassForPlanDTO> classesDTOS = new List<ClassForPlanDTO>()
            {
                new ClassForPlanDTO(3, "Kick-Boxing", new List<string>{"Martial Arts"}, today.AddDays(4).AddHours(17).AddMinutes(30), 19.00m),
                new ClassForPlanDTO(2, "Meditation", new List<string>{"Meditation"}, today.AddDays(2).AddHours(10), 8.00m),
                new ClassForPlanDTO(1, "Strength Training", new List<string>{"Strength"}, today.AddDays(1).AddHours(15), 20.00m),
                new ClassForPlanDTO(4, "Zumba", new List<string>{"Dance"}, today.AddDays(3).AddHours(18).AddMinutes(30), 12.00m)
            };

            var mock = new Mock<ILogger<ClassController>>();
            ILogger<ClassController> logger = mock.Object;

            var controller = new ClassController(_context, logger);

            // Act
            var result = await controller.GetClassesForPlanning(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var classDTOsActual = Assert.IsType<List<ClassForPlanDTO>>(okResult.Value);

            Assert.Equal(classesDTOS.Count, classDTOsActual.Count);
        }

        // Test that the endpoint returns BadRequest when a past date is provided
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetClassesForPlanning_date_in_past_returns_BadRequest()
        {
            // Arrange
            var pastDate = DateTime.Today.AddDays(-1); //Date in the past

            var mock = new Mock<ILogger<ClassController>>();
            ILogger<ClassController> logger = mock.Object;
            var controller = new ClassController(_context, logger);

            // Act
            var result = await controller.GetClassesForPlanning(pastDate, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // Test that the endpoint returns NotFound when no classes are available for the given criteria
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetClassesForPlanning_no_classes_returns_NotFound()
        {
            // Arrange
            var futureDate = DateTime.Today.AddDays(10); // Date that is outside of the next week range

            var mock = new Mock<ILogger<ClassController>>();
            ILogger<ClassController> logger = mock.Object;
            var controller = new ClassController(_context, logger);

            // Act
            var result = await controller.GetClassesForPlanning(futureDate, null);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // Test that the endpoint returns only classes matching the specified date
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetClassesForPlanning_filter_by_date()
        {
            // Arrange
            var targetDate = DateTime.Today.AddDays(2); // date used in the filter

            var mock = new Mock<ILogger<ClassController>>();
            ILogger<ClassController> logger = mock.Object;
            var controller = new ClassController(_context, logger);

            // Act
            var result = await controller.GetClassesForPlanning(targetDate, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualClasses = Assert.IsType<List<ClassForPlanDTO>>(okResult.Value);

            Assert.All(actualClasses, c => Assert.Equal(targetDate.Date, c.DateTime.Date));
        }

        // Test that the endpoint returns only classes matching the specified types
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetClassesForPlanning_filter_by_types()
        {
            // Arrange
            var types = new string[] { "Dance", "Martial Arts" }; // types used in the filter

            var mock = new Mock<ILogger<ClassController>>();
            ILogger<ClassController> logger = mock.Object;
            var controller = new ClassController(_context, logger);

            // Act
            var result = await controller.GetClassesForPlanning(null, types);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualClasses = Assert.IsType<List<ClassForPlanDTO>>(okResult.Value);

            Assert.All(actualClasses, c => Assert.Contains(c.TypeItemNames[0], types));
        }

        // Test that the endpoint returns only classes matching both the specified date and types
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetClassesForPlanning_filter_by_date_and_types()
        {
            // Arrange
            var types = new string[] { "Strength" }; // types used in the filter
            var date = DateTime.Today.AddDays(1);    // date used in the filter

            var mock = new Mock<ILogger<ClassController>>();
            ILogger<ClassController> logger = mock.Object;
            var controller = new ClassController(_context, logger);

            // Act
            var result = await controller.GetClassesForPlanning(date, types);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualClasses = Assert.IsType<List<ClassForPlanDTO>>(okResult.Value);

            // Check date
            Assert.All(actualClasses, c => Assert.Equal(date.Date, c.DateTime.Date));

            // Check type 
            Assert.All(actualClasses, c => Assert.Contains(c.TypeItemNames[0], types));
        }


    }
}