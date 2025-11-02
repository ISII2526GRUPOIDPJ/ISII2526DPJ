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
                        pi.Class.Id,
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

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(GetPlanDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreatePlan(CreatePlanDTO planDto)
        {
            try
            {
                // Alternative Flow 4: No classes selected
                if(planDto.SelectedClasses == null || !planDto.SelectedClasses.Any())
                {
                    return BadRequest("At least one class must be selected.");
                }

                // Alternative Flow 5: Mandatory data validation
                if (string.IsNullOrEmpty(planDto.Name))
                {
                    return BadRequest("Plan name is required");
                }

                if(planDto.Weeks <= 0)
                {
                    return BadRequest("Number of weeks must be greater than 0.");
                }

                if(planDto.SelectedPaymentMethodId <= 0)
                {
                    return BadRequest("Payment method is required.");
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
                    HealthIssues = planDto.HealthIssues != null ? planDto.HealthIssues : string.Empty,
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


                return Ok(resultDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating plan");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error creating plan");
            }
        }
    }
}
