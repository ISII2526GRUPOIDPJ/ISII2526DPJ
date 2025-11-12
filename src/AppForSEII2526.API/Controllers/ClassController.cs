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

            _logger.LogInformation("ClassController initialized.");
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ClassForPlanDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetClassesForPlanning(DateTime? date, string[]? types)
        {
            _logger.LogInformation("GetClassesForPlanning initialized with date: {date} and types: {types}.", date, types);

            try
            {
                // Alternative flow 2: Date Validation (not before today)
                if (date.HasValue && date.Value.Date < DateTime.Today)
                {
                    _logger.LogWarning("Date validation failed, date is in the past: {date}", date);
                    return BadRequest("Cannot select classes from past dates.");
                }

                _logger.LogDebug("Date validation passed, proceeding with query.");

                // Calculate next week range (Step 2 requirement)
                var startDate = DateTime.Today;
                var endDate = DateTime.Today.AddDays(7);

                _logger.LogDebug("Querying classes for date range: {StartDate} to {EndDate}.", startDate, endDate);

                IList<ClassForPlanDTO> classesDTOS = await _context.Classes
                    .Include(c => c.TypeItems)
                    .Where(c => c.Date >= startDate && c.Date <= endDate) // Next week only
                    .Where(c => !date.HasValue || c.Date.Date == date.Value.Date) // Date filter
                    .Where(c => types == null || !types.Any() || c.TypeItems.Any(ti => types.Contains(ti.Name))) // Type filter
                    .OrderBy(c => c.Name)
                    .Select(c => new ClassForPlanDTO(
                        c.Id,
                        c.Name,
                        c.TypeItems.Select(ti => ti.Name).ToList(),
                        c.Date,
                        c.Price
                    ))
                    .ToListAsync();

                _logger.LogDebug("Database query completed. Found {ClassCount} classes", classesDTOS.Count);

                // Alternative flow 0: No classes available warning
                if (!classesDTOS.Any())
                {
                    _logger.LogWarning("No classes found for the selected criteria: date={date}, types={types}", date, types);
                    return NotFound("No classes found for the selected criteria.");
                }

                _logger.LogInformation("GetClassesForPlanning completed successfully with {ClassCount} classes found.", classesDTOS.Count);
                return Ok(classesDTOS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving classes for planning.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

            //[HttpGet]
            //[Route("[action]")]
            //[ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
            //[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
            //public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
            //{
            //if (op2 == 0)
            //{
            //    string error = "Op2 cannot be 0 to compute a division";
            //    _logger.LogError(DateTime.Now + " Error: "+ error);
            //    return BadRequest(error);
            //}

            //decimal result = op1 / op2;
            //return Ok(result);
            //}
        }
    }
}
