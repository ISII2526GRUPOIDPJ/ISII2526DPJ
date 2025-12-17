using AppForSEII2526.API.DTOs.ClassDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ClassController> _logger;

        public ClassController(ApplicationDbContext context, ILogger<ClassController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ClassForPlanDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetClassesForPlanning(DateTime? date, string? type, decimal maximumprice)
        {
            // Alternative flow 2: Date Validation (not before today)
            if (date.HasValue && date.Value.Date < DateTime.Today)
            {
                return BadRequest("Cannot select classes from past dates.");
            }

            // If specific date: search only that day | If no date: search next week
            // Filter by types if provided, otherwise get all types
            var startDate = date.HasValue ? date.Value.Date : DateTime.Today;
            var endDate = date.HasValue ? date.Value.Date.AddDays(1) : DateTime.Today.AddDays(7);

            if(maximumprice <= 0)
            {
                return BadRequest("Maximum price cannot be nullable.");
            }

            IList<ClassForPlanDTO> classesDTOS = await _context.Classes
                .Include(c => c.TypeItems)
                .Where(c => (c.Date >= startDate && c.Date < endDate) && maximumprice >= c.Price)
                .Where(c => (string.IsNullOrEmpty(type) || c.TypeItems.Any(ti => ti.Name == type)) && maximumprice >= c.Price)
                .OrderBy(c => c.Date)
                    .ThenBy(c => c.Name)
                .Select(c => new ClassForPlanDTO(
                    c.Id,
                    c.Name,
                    c.TypeItems.Select(ti => ti.Name).ToList(),
                    c.Date,
                    c.Price
                ))
                .ToListAsync();

            if (!classesDTOS.Any())
            {
                return NotFound("No classes found for the selected criteria.");
            }

            return Ok(classesDTOS);
        }
    }
}
