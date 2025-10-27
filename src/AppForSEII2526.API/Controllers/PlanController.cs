using AppForSEII2526.API.DTOs.ClassDTOs;
using AppForSEII2526.API.DTOs.PlanDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<PlanController> _logger;

        public PlanController(ApplicationDbContext context, ILogger<PlanController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<GetPlanDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetPlan(DateTime? date)
        {
            IList<GetPlanDTO> planDTOs = await _context.Plans
                .Include(p => p.PaymentMethod)
                    .ThenInclude(pm => pm.User)
                .Include(p => p.PlanItems)
                    .ThenInclude(pi => pi.Class)
                        .ThenInclude(c => c.TypeItems)
                .Where(p => !date.HasValue || p.CreatedDate.Date == date.Value.Date)
                .OrderBy(p => p.CreatedDate)
                .Select(p => new GetPlanDTO(
                    p.PaymentMethod.User.Name,
                    p.PaymentMethod.User.Surname,
                    p.CreatedDate,
                    p.TotalPrice,
                    p.Name,
                    p.Description != null ? p.Description : string.Empty,
                    p.Weeks,
                    p.HealthIssues,
                    p.PlanItems.Select(pi => new ClassInPlanDTO(
                        pi.Class.Name,
                        pi.Class.TypeItems.Select(ti => ti.Name).ToList(),
                        pi.Price,
                        pi.Class.Date,
                        pi.Goal
                    )).ToList()
                ))
                .ToListAsync();

            return Ok(planDTOs);
        }


    }
}
