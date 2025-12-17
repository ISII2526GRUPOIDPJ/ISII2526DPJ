using AppForSEII2526.API.DTOs.ItemDTOs;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ItemsController> _logger;

        public ItemsController(ApplicationDbContext context, ILogger<ItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ItemForPurchaseDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetItemsForPurchase(string? itemName, string? brandName, int minquantity) {
            IList<ItemForPurchaseDTO> items = await _context.Items
                .Include(i => i.PurchaseItems)
                .Where(i => (i.Name.Contains(itemName) || (itemName == null)) && (i.Brand.Name.Equals(brandName) || (brandName == null)) && (i.QuantityAvailableForPurchase >= minquantity))
                .Select(i => new ItemForPurchaseDTO(
                    i.Name,
                    i.Brand.Name,
                    i.Description,
                    i.PurchasePrice,
                    i.QuantityAvailableForPurchase)
                )
                .ToListAsync();
            return Ok(items);
        }
    }
}
