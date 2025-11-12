using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
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

            _logger.LogInformation("PurchaseController initialized.");
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PurchaseDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetPurchase(int id)
        {
            PurchaseDTO? purchase = await _context.Purchases
                .Include(p => p.PurchaseItems)
                    .ThenInclude(pi => pi.Item)
                        .ThenInclude(i => i.Brand)
                .Where(p => p.Id == id)
                .Select(p => new PurchaseDTO(
                    p.City,
                    p.Country,
                    p.Street,
                    p.Total_price,
                    p.Description,
                    p.PaymentMethod,
                    p.PurchaseItems.Select(pi => new PurchaseItemsDTO(pi.Item.Name, pi.Item.Brand.Name, pi.Item.QuantityAvailableForPurchase, pi.Item.PurchasePrice)).ToList()
                ))
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
