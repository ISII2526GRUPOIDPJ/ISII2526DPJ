using AppForSEII2526.API.DTOs.ItemDTOs;
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

        /*[HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        {
            if(op2 == 0) {
                string error = "You can't divide by zero.";
                _logger.LogError("[" + DateTime.Now + "] Error: " + error);
                return BadRequest(error);
            }
            decimal result = op1 / op2;
            return Ok(result);
        }*/

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ItemForPurchaseDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetItemsForPurchase(string? itemName, string? brandName)
        {
            IList<ItemForPurchaseDTO> items = await _context.Items
                .Include(i => i.Name)
                .Include(i => i.PurchaseItems)
                .Where(i => (i.Name.Contains(itemName) || (itemName == null)) && (i.Brand.Name.Equals(brandName) || (brandName == null)))
                .OrderBy(i => i.Name)
                .Select(i => new ItemForPurchaseDTO(i.Name, i.Brand.Name, i.Description, i.PurchaseItems.Select(pi => pi.Price).ToList(), i.QuantityAvailableForPurchase))
                .ToListAsync();
            return Ok(items);
        }
    }
}
