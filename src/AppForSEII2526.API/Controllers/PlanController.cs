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
        [ProducesResponseType(typeof(GetPlanDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPlan(int id)
        {
            if (_context.Plans == null)
                return NotFound();

            var plan = await _context.Plans
                .Include(p => p.PaymentMethod)
                    .ThenInclude(pm => pm.User)
                .Include(p => p.PlanItems)
                    .ThenInclude(pi => pi.Class)
                        .ThenInclude(c => c.TypeItems)
                .Where(p => p.Id == id)
                .Select(p => new GetPlanDTO(
                    p.Id,
                    p.PaymentMethod.User.Name,
                    p.PaymentMethod.User.Surname,
                    p.CreatedDate,
                    p.TotalPrice,
                    p.Name,
                    p.Description ?? string.Empty,
                    p.Weeks,
                    p.HealthIssues,
                    p.PlanItems.Select(pi => new ClassInPlanDTO(
                        pi.Class.Id,
                        pi.Class.Name,
                        pi.Class.TypeItems.Select(ti => ti.Name).ToList(),
                        pi.Price,
                        pi.Class.Date,
                        pi.Goal
                    )).ToList()
                ))
                .FirstOrDefaultAsync();

            if (plan == null)
                return NotFound();

            return Ok(plan);
        }


        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(GetPlanDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreatePlan(CreatePlanDTO planDto)
        {
            try
            {
                // Included because in the tests the [ApiController] automatic validation does not trigger
                if (string.IsNullOrWhiteSpace(planDto.Name)) return BadRequest("Plan name is required");
                if (planDto.Weeks < 1 || planDto.Weeks > 52) return BadRequest("Weeks must be between 1 and 52");


                // Alternative Flow 4: No classes selected
                if (planDto.SelectedClasses == null || !planDto.SelectedClasses.Any())
                {
                    return BadRequest("At least one class must be selected.");
                }

                // Alternative Flow 7: Check class capacity
                var classIds = planDto.SelectedClasses.Select(sc => sc.Id).ToList();
                var classesWithoutCapacity = await _context.Classes
                    .Where(c => classIds.Contains(c.Id) && c.Capacity <= 0)
                    .Select(c => c.Name)
                    .ToListAsync();

                if (classesWithoutCapacity.Any())
                {
                    return BadRequest($"The following classes have no available capacity: {string.Join(", ", classesWithoutCapacity)}");
                }

                // Calculate total price
                var prices = await _context.Classes
                    .Where(c => classIds.Contains(c.Id))
                    .Select(c => c.Price)
                    .ToListAsync();

                decimal totalPrice = prices.Sum();

                // Validate that the selected payment method exists, preventing creation with invalid payment method
                var paymentMethod = await _context.PaymentMethods.FindAsync(planDto.SelectedPaymentMethodId);
                if (paymentMethod == null)
                {
                    return BadRequest("Selected payment method not found.");
                }

                // Create Plan entity with the information introduced by the user
                var plan = new Plan
                {
                    Name = planDto.Name,
                    Description = planDto.Description,
                    Weeks = planDto.Weeks,
                    HealthIssues = planDto.HealthIssues ?? string.Empty,
                    TotalPrice = totalPrice,
                    CreatedDate = DateTime.Now,
                    PaymentMethod = paymentMethod
                };

                _context.Plans.Add(plan);
                await _context.SaveChangesAsync();

                // Create PlanItems with goals
                var planItems = planDto.SelectedClasses.Select(sc => new PlanItem
                {
                    Plan = plan,
                    ClassId = sc.Id,
                    Goal = sc.Goal != null ? sc.Goal : string.Empty,
                    Price = sc.Price
                }).ToList();

                _context.PlanItems.AddRange(planItems);
                await _context.SaveChangesAsync();

                // Return the created plan DTO
                var resultDto = await _context.Plans
                    .Include(p => p.PaymentMethod)
                        .ThenInclude(pm => pm.User)
                    .Include(p => p.PlanItems)
                        .ThenInclude(pi => pi.Class)
                            .ThenInclude(c => c.TypeItems)
                    .Where(p => p.Id == plan.Id)
                    .Select(p => new GetPlanDTO(
                        p.Id,
                        p.PaymentMethod.User.Name,
                        p.PaymentMethod.User.Surname,
                        p.CreatedDate,
                        p.TotalPrice,
                        p.Name,
                        p.Description != null ? p.Description : string.Empty,
                        p.Weeks,
                        p.HealthIssues,
                        p.PlanItems.Select(pi => new ClassInPlanDTO(
                            pi.Class.Id,
                            pi.Class.Name,
                            pi.Class.TypeItems.Select(ti => ti.Name).ToList(),
                            pi.Price,
                            pi.Class.Date,
                            pi.Goal
                        )).ToList()
                    ))
                    .FirstOrDefaultAsync();


                return CreatedAtAction(nameof(GetPlan), new { id = plan.Id }, resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error creating plan");
            }
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<PaymentMethodDTO>>> GetPaymentMethods()
        {
            var methods = await _context.PaymentMethods.ToListAsync();
            return Ok(methods);
        }
    }
}
