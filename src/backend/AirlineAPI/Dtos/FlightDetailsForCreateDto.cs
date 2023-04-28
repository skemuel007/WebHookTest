namespace AirlineAPI.Dtos
{
    public class FlightDetailsForCreateDto
    {
        public string FlightCode { get; set; } = default!;
        public decimal Price { get; set; }
    }
}
