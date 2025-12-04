namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ClassController> _logger;

        public TypesController(ApplicationDbContext context, ILogger<ClassController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllClassTypes()
        {
            try
            {
                IList<string> types = await _context.TypeItems
                    .OrderBy(t => t.Name)
                    .Select(t => t.Name)
                    .Distinct()
                    .ToListAsync();

                if (types == null || types.Count == 0)
                {
                    return Ok(new List<string>());
                }

                return Ok(types);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting class types");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
