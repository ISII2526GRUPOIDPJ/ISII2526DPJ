using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.Models;
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
        [ProducesResponseType(typeof(PurchaseDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetPurchase(int id)
        {
            if(_context.Purchases == null) {
                return NotFound();
            }

            var purchase = await _context.Purchases
                .Include(p => p.PurchaseItems)
                    .ThenInclude(pi => pi.Item)
                        .ThenInclude(b => b.Brand)
                .Include(p => p.PaymentMethod)
                .Where(p => p.Id == id)
                .Select(p => new PurchaseDTO(
                    p.Id,
                    p.City,
                    p.Country,
                    p.Street,
                    p.Total_price,
                    p.Description,
                    new PaymentMethodDTO(p.PaymentMethod.Id, p.PaymentMethod.GetType().Name, p.PaymentMethod.Description),
                    p.PurchaseItems.Select(pi => new PurchaseItemsDTO(pi.Item.Name, pi.Item.Brand.Name, pi.Item.Description, pi.Amount_bought, pi.Price)).ToList()
                ))
                .FirstOrDefaultAsync();

            if (purchase == null) {
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
            if(createPurchase.PaymentMethod == null) ModelState.AddModelError("PaymentMethod", "Payment method is required");
            if(createPurchase.PurchaseItems == null || !createPurchase.PurchaseItems.Any()) ModelState.AddModelError("PurchaseItems", "At least one item must be selected");

            if (ModelState.ErrorCount > 0) {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var itemNames = createPurchase.PurchaseItems.Select(pi => pi.Name).ToList<string>();

            var items = _context.Items.Include(i => i.PurchaseItems)
                .ThenInclude(pi => pi.Purchase)
                .Where(i => itemNames.Contains(i.Name))
                .Select(i => new {
                    i.Id,
                    i.Name,
                    i.QuantityAvailableForPurchase,
                    i.PurchasePrice
                })
                .ToList();

            var paymentMethod = await _context.PaymentMethods.FindAsync(createPurchase.PaymentMethod.Id);

            Purchase purchase = new Purchase(createPurchase.City, createPurchase.Country, createPurchase.Street, createPurchase.Date, createPurchase.Description, createPurchase.Total_price, new List<PurchaseItem>(), paymentMethod);

            foreach(var i in createPurchase.PurchaseItems) {
                var item = items.FirstOrDefault(m => m.Name == i.Name);
                if ((item == null) || (i.Quantity > item.QuantityAvailableForPurchase)) {
                    ModelState.AddModelError("PurchaseItems", $"Error! There's no stock for '{i.Name}'.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                } else {
                    purchase.PurchaseItems.Add(new PurchaseItem(item.Id, i.Quantity, item.PurchasePrice, purchase));
                    i.Price = item.PurchasePrice;
                }
            }

            purchase.Total_price = purchase.PurchaseItems
                .Sum(pi => pi.Amount_bought * pi.Price);

            try {
                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();
            } catch (Exception ex) {
                return Conflict("Error" +  ex.Message);
            }

            var purchaseDetail = await _context.Purchases
                .Include(p => p.PurchaseItems)
                    .ThenInclude(pi => pi.Item)
                        .ThenInclude(b => b.Brand)
                .Include(p => p.PaymentMethod)
                .Where(p => p.Id == purchase.Id)
                .Select(p => new PurchaseDTO(
                    p.Id,
                    p.City,
                    p.Country,
                    p.Street,
                    p.Total_price,
                    p.Description,
                    new PaymentMethodDTO(p.PaymentMethod.Id, p.PaymentMethod.GetType().Name, p.PaymentMethod.Description),
                    p.PurchaseItems.Select(pi => new PurchaseItemsDTO(
                        pi.Item.Name,
                        pi.Item.Brand.Name,
                        pi.Item.Description,
                        pi.Amount_bought,
                        pi.Price
                    )).ToList()
                ))
                .FirstOrDefaultAsync();

            return CreatedAtAction("GetPurchase", new {id = purchase.Id}, purchaseDetail);
        }
    }
}
