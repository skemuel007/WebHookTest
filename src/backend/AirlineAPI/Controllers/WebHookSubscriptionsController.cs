using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirlineAPI.Models;
using AirlineAPI.Data;
using AirlineAPI.Dtos;
using AutoMapper;

namespace AirlineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHookSubscriptionsController : ControllerBase
    {
        private readonly AirlineDBContext _context;
        private readonly IMapper _mapper;

        public WebHookSubscriptionsController(AirlineDBContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/WebHookSubscriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WebHookSubscriptionResponsDto>>> GetWebHookSubscription()
        {
            if (_context.WebHookSubscription == null)
            {
                return NotFound();
            }
            var result =  await _context.WebHookSubscription.ToListAsync();
            
            return _mapper.Map<List<WebHookSubscriptionResponsDto>>(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WebHookSubscriptionResponsDto>> GetWebHookSubscription(int id)
        {
            var webHookSubscription = await _context.WebHookSubscription.FindAsync(id);

            if (webHookSubscription == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WebHookSubscriptionResponsDto>(webHookSubscription));
        }


        [HttpPost]
        public async Task<ActionResult<WebHookSubscriptionResponsDto>> CreateWebHookSubscription(WebHookSubscriptionCreateDto webHookSubscriptionCreateDto)
        {
            var subscription = _context.WebHookSubscription.FirstOrDefault(s => s.WebHookUri ==  webHookSubscriptionCreateDto.WebhookUri);

            if (subscription is null)
            {
                subscription = _mapper.Map<WebHookSubscription>(webHookSubscriptionCreateDto);
                subscription.Secret = Guid.NewGuid().ToString();
                subscription.WebHookPublisher = "PanAus";

                try
                {
                    _context.WebHookSubscription.Add(subscription);
                    await _context.SaveChangesAsync();

                } catch(Exception ex)
                {
                    return Problem("Error creating webhook subscription");
                }

                var webHookSubscriptionResponseDto = _mapper.Map<WebHookSubscriptionResponsDto>(subscription);

                return CreatedAtRoute(nameof(GetSubscriptionBySecret), new { secret = webHookSubscriptionResponseDto.Secret }, webHookSubscriptionResponseDto);

            } else
            {
                return Problem("Webhook already registered");
            }

            
        }

        [HttpGet("{secret}", Name = "GetSubscriptionBySecret")]
        public async Task<ActionResult<WebHookSubscriptionResponsDto>> GetSubscriptionBySecret(string secret)
        {
            var subscription = await _context.WebHookSubscription.FirstOrDefaultAsync(s => s.Secret == secret);

            if (subscription == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WebHookSubscriptionResponsDto>(subscription));
        }

        // DELETE: api/WebHookSubscriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWebHookSubscription(int id)
        {
            if (_context.WebHookSubscription == null)
            {
                return NotFound();
            }
            var webHookSubscription = await _context.WebHookSubscription.FindAsync(id);
            if (webHookSubscription == null)
            {
                return NotFound();
            }

            _context.WebHookSubscription.Remove(webHookSubscription);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
