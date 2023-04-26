using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirlineAPI.Models;
using AirlineAPI.Data;

namespace AirlineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHookSubscriptionsController : ControllerBase
    {
        private readonly AirlineDBContext _context;

        public WebHookSubscriptionsController(AirlineDBContext context)
        {
            _context = context;
        }

        // GET: api/WebHookSubscriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WebHookSubscription>>> GetWebHookSubscription()
        {
            if (_context.WebHookSubscription == null)
            {
                return NotFound();
            }
            return await _context.WebHookSubscription.ToListAsync();
        }

        // GET: api/WebHookSubscriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WebHookSubscription>> GetWebHookSubscription(int id)
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

            return webHookSubscription;
        }

        // PUT: api/WebHookSubscriptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWebHookSubscription(int id, WebHookSubscription webHookSubscription)
        {
            if (id != webHookSubscription.Id)
            {
                return BadRequest();
            }

            _context.Entry(webHookSubscription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WebHookSubscriptionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WebHookSubscriptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WebHookSubscription>> PostWebHookSubscription(WebHookSubscription webHookSubscription)
        {
            if (_context.WebHookSubscription == null)
            {
                return Problem("Entity set 'AirlineDBContext.WebHookSubscription'  is null.");
            }
            _context.WebHookSubscription.Add(webHookSubscription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWebHookSubscription", new { id = webHookSubscription.Id }, webHookSubscription);
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

        private bool WebHookSubscriptionExists(int id)
        {
            return (_context.WebHookSubscription?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
