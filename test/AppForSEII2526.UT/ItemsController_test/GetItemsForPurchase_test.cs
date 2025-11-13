using AppForSEII2526.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ItemDTOs;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ItemsController_test
{
    public class GetItemsForPurchase_test : AppForSEII25264SqliteUT
    {
        public GetItemsForPurchase_test()
        {
            var brands = new List<Brand>() {
                    new Brand("Nike"),
                    new Brand("Adidas")
            };

            var types = new List<TypeItem>() {
                    new TypeItem("Yoga"),
                    new TypeItem("Cardio"),
                    new TypeItem("Strenght")
            };

            var items = new List<Item>() {
                    new Item("Yoga mat for exercises", "Yoga Mat", 25, 10, 5, 20, types[0], brands[0]),
                    new Item("Running Shoes", "Running Shoes", 80, 15, 8, 70, types[1], brands[1]),
                    new Item("Shirt for doing exercises", "Sports Shirt", 100, 0, 6, 85, types[2], brands[0])
            };

            _context.AddRange(brands);
            _context.AddRange(items);
            _context.SaveChanges();
        }

        public static IEnumerable<Object[]> TestCasesFor_GetItemsForPurchase_OK() {
            var itemDTOs = new List<ItemForPurchaseDTO>() {
                new ItemForPurchaseDTO("Yoga Mat", "Nike", "Yoga mat for exercises", 25, 10),
                new ItemForPurchaseDTO("Running Shoes", "Adidas", "Running Shoes", 80, 15),
                new ItemForPurchaseDTO("Sports Shirt", "Nike", "Shirt for doing exercises", 100, 0)
            };

            var itemDTOsTC1 = new List<ItemForPurchaseDTO>() { itemDTOs[0], itemDTOs[1], itemDTOs[2] };
            var itemDTOsTC2 = new List<ItemForPurchaseDTO>() { itemDTOs[1] };
            var itemDTOsTC3 = new List<ItemForPurchaseDTO>() { itemDTOs[0], itemDTOs[2] };

            var allTests = new List<Object[]> {
                new object[] { null, null, itemDTOsTC1,  },
                new object[] { "Shoes", null, itemDTOsTC2, },
                new object[] { null, "Nike", itemDTOsTC3, },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetItemsForPurchase_OK))]
        public async Task GetItemsForPurchase_filter_test(string? itemName, string? itemBrand, List<ItemForPurchaseDTO> expectedItems)
        {
            //Arrange
            var controller = new ItemsController(_context, null);

            //Act
            var result = await controller.GetItemsForPurchase(itemName, itemBrand);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var itemDTOsActual = Assert.IsType<List<ItemForPurchaseDTO>>(okResult.Value);
            Assert.Equal(expectedItems, itemDTOsActual);
        }
    }
}