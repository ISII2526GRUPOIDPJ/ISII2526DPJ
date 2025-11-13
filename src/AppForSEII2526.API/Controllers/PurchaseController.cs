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

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(PurchaseDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreatePurchase(CreatePurchaseDTO createPurchase) {
            //Mandatory information not introduced
            if (string.IsNullOrWhiteSpace(createPurchase.Street)) ModelState.AddModelError("PurchaseCountry", "Street is required");
            if (string.IsNullOrWhiteSpace(createPurchase.City)) ModelState.AddModelError("PurchaseCountry", "City is required");
            if (string.IsNullOrWhiteSpace(createPurchase.Country)) ModelState.AddModelError("PurchaseCountry", "Country is required");

            var itemNames = createPurchase.PurchaseItems.Select(pi => pi.Name).ToList<string>();

            var items = _context.Items.Include(i => i.PurchaseItems)
                .ThenInclude(pi => pi.Purchase)
                .Where(i => itemNames.Contains(i.Name))
                .Select(i => new {
                    i.Id,
                    i.Name,
                    i.QuantityAvailableForPurchase,
                    i.PurchasePrice,
                    Amount = i.PurchaseItems.Count()
                })
                .ToList();

            Purchase purchase = new Purchase(createPurchase.City, createPurchase.Country, createPurchase.Street, createPurchase.Date, createPurchase.Description, createPurchase.Total_price, new List<PurchaseItem>(), createPurchase.PaymentMethod);

            foreach(var i in createPurchase.PurchaseItems) {
                var item = items.FirstOrDefault(m => m.Name == i.Name);
                if ((item == null) || (item.Amount >= item.QuantityAvailableForPurchase)) {
                    ModelState.AddModelError("PurchaseItems", $"Error! Item named '{i.Name}' isn't available for being purchased.");
                } else {
                    purchase.PurchaseItems.Add(new PurchaseItem(item.Id, item.Amount, item.PurchasePrice, purchase));
                    i.Price = item.PurchasePrice;
                }
            }

            if(ModelState.ErrorCount > 0) {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            try {
                await _context.SaveChangesAsync();
            } catch (Exception ex) {
                _logger.LogError(DateTime.Now + ":" + ex.Message);
                return Conflict("Error" +  ex.Message);
            }

            var purchaseDetail = new PurchaseDTO(purchase.City, purchase.Country, purchase.Street, purchase.Total_price, purchase.Description, purchase.PaymentMethod, createPurchase.PurchaseItems);

            return CreatedAtAction("GetPurchase", new {id = purchase.Id}, purchaseDetail);
        }
    }
}
