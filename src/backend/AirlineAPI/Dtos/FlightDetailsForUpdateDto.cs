namespace AirlineAPI.Dtos
{
    public class FlightDetailsForUpdateDto
    { 
        public string FlightCode { get; set; } = default!;
        public decimal Price { get; set; }
    }
}
