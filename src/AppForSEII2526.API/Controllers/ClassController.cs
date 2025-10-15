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
        public async Task<ActionResult> GetClassesForPlanning(DateTime? date)
        {
            IList<ClassForPlanDTO> classesDTOS = await _context.Classes
                .Include(c => c.TypeItems)
                .Where(c => !date.HasValue || c.Date.Date == date.Value.Date)
                .OrderBy(c => c.Name)
                .Select(c=>new ClassForPlanDTO(
                    c.Id, 
                    c.Name, 
                    c.TypeItems.Select(ti => ti.Name).ToList()
                ))
                .ToListAsync();
            return Ok(classesDTOS);
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
