using AppForMovies.UT;
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

            var items = new List<Item>() {
                    new Item("Yoga mat for exercises", "Yoga Mat", 25, 10, 5, 20, brands[0]),
                    new Item("Running Shoes", "Running Shoes", 80, 15, 8, 70, brands[1]),
                    new Item("Shirt for doing exercises", "Sports Shirt", 100, 0, 6, 85, brands[0])
            };

            _context.AddRange(brands);
            _context.AddRange(items);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetItemsForPurchaseNULL4NameBrand_test() {
            //Arrange
            List<ItemForPurchaseDTO> expectedItems = new List<ItemForPurchaseDTO>() {
                new ItemForPurchaseDTO("Yoga Mat", "Nike", "Yoga mat for exercises", 25, 10),
                new ItemForPurchaseDTO("Running Shoes", "Adidas", "Running Shoes", 80, 15),
                new ItemForPurchaseDTO("Sports Shirt", "Nike", "Shirt for doing exercises", 100, 0)
            };

            ItemsController controller = new ItemsController(_context, null);

            //Act
            var result = await controller.GetItemsForPurchase(null, null);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var itemDTOsActual = Assert.IsType<List<ItemForPurchaseDTO>>(okResult.Value);
            Assert.Equal(expectedItems, itemDTOsActual);
        }
    }
}
