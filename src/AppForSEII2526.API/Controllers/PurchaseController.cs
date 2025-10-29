using AppForSEII2526.API.DTOs.ItemDTOs;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<PurchaseController> _logger;

        public PurchaseController(ApplicationDbContext context, ILogger<PurchaseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PurchaseDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetPurchase(int id)
        {
            IList<PurchaseDTO>? purchase = await _context.Purchases
                .OrderBy(i => i.Id)
                .Select(p => new PurchaseDTO(
                    p.City,
                    p.Country,
                    p.Street,
                    p.Total_price,
                    p.Description,
                    p.PaymentMethod,
                    p.PurchaseItems.Select(pi => new PurchaseItemsDTO(pi.Item.Name, pi.Item.Brand.Name, pi.Item.QuantityAvailableForPurchase, pi.Item.PurchasePrice)).ToList(),
                )
                .FirstOrDefaultAsync();

            if (purchase == null)
            {
                _logger.LogError($"Error: Purchase with the ID {id} does not exist.");
                return NotFound();
            }

            return Ok(purchase);
        }
    }
}
