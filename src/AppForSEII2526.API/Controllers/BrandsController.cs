namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ItemsController> _logger;

        public BrandsController(ApplicationDbContext context, ILogger<ItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllItemBrands()
        {
            try
            {
                IList<string> brands = await _context.Brands
                    .OrderBy(b => b.Name)
                    .Select(b => b.Name)
                    .Distinct()
                    .ToListAsync();

                if (brands == null || brands.Count == 0)
                {
                    return Ok(new List<string>());
                }

                return Ok(brands);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
