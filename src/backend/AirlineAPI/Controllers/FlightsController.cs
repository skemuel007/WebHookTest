using AirlineAPI.Data;
using AirlineAPI.Dtos;
using AirlineAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirlineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly AirlineDBContext _dbContext;
        private readonly IMapper _mapper;
        public FlightsController(AirlineDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("{flightCode}", Name = "GetFlightDetailsByCode")]
        public async Task<ActionResult<FlightDetailsResponseDto>> GetFlightDetailsByCode(string flightCode)
        {
            var flight = _dbContext.FlightDetail.SingleOrDefault(x => x.FlightCode == flightCode);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FlightDetailsResponseDto>(flight));
        }

        [HttpPost]
        public async Task<ActionResult<FlightDetailsResponseDto>> CreateFlight(FlightDetailsForCreateDto flightDetailsForCreateDto)
        {
            var flight = _dbContext.FlightDetail.SingleOrDefault(x => x.FlightCode == flightDetailsForCreateDto.FlightCode);

            if (flight == null)
            {
                var flightDetailModel = _mapper.Map<FlightDetail>(flightDetailsForCreateDto);

                try
                {
                    await _dbContext.FlightDetail.AddAsync(flightDetailModel);
                    await _dbContext.SaveChangesAsync();
                }catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                var flightResponseDto = _mapper.Map<FlightDetailsResponseDto>(flightDetailModel);

                return CreatedAtRoute(nameof(GetFlightDetailsByCode), new { flightCode =  flightDetailsForCreateDto.FlightCode }, flightResponseDto);
            } else
            {
                return BadRequest("Flight record alread exists");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFlightDetails(int id, [FromBody] FlightDetailsForUpdateDto flightDetailsForUpdateDto)
        {
            var flight = await _dbContext.FlightDetail.FindAsync(id);

            if (flight is null)
            {
                return NotFound();
            }

            _mapper.Map(flightDetailsForUpdateDto, flight);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
