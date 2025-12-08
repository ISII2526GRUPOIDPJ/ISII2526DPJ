namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NamesController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ItemsController> _logger;

        public NamesController(ApplicationDbContext context, ILogger<ItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllItemNames()
        {
            try
            {
                IList<string> names = await _context.Items
                    .OrderBy(n => n.Name)
                    .Select(n => n.Name)
                    .Distinct()
                    .ToListAsync();

                if (names == null || names.Count == 0)
                {
                    return Ok(new List<string>());
                }

                return Ok(names);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
