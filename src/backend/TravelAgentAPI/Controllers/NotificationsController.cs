using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelAgentAPI.Data;
using TravelAgentAPI.Dtos;

namespace TravelAgentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly TravelAgentDbContext _context;
        private ILogger<NotificationsController> _logger;

        public NotificationsController(
            TravelAgentDbContext context,
            ILogger<NotificationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> FlightChanged(FlightDetailUpdateDto flightDetailUpdateDto)
        {
            _logger.LogInformation($"Webhook Received from: {flightDetailUpdateDto.Publisher}");
            var secretModel = _context.SubscriptionSecret.FirstOrDefault( s => s.Publisher == flightDetailUpdateDto.Publisher 
                && s.Secret == flightDetailUpdateDto.Secret);

            if ( secretModel is null)
            {
                _logger.LogInformation($"Invalid Secret - Ignore Webhook");
                return NotFound();
            }

            _logger.LogInformation($"Valid webhook - Old Price {flightDetailUpdateDto.OldPrice} and New Price: {flightDetailUpdateDto.NewPrice}");
            return Ok();
        }
    }
}
