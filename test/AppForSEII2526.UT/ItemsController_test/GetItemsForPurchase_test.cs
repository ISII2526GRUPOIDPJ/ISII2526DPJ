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
                    new Brand(1, "Nike"),
                    new Brand(2, "Adidas"),
                    new Brand(3, "Reebok"),
                    new Brand(4, "Under Armour")
            };

            var items = new List<Item>() {
                    new Item(1, "Yoga mat for exercises", "Yoga Mat", 25, 10, 5, 20, brands[0]),
                    new Item(2, "Running Shoes", "Running Shoes", 80, 15, 8, 70, brands[1])
            };

            _context.AddRange(brands);
            _context.AddRange(items);
            _context.SaveChanges();
        }

        [Route("api/[controller]")]
        [ApiController]
        public class ItemsController
        {
            private ApplicationDbContext _context;
            private ILogger<ItemsController> _logger;
        }
    }
}
