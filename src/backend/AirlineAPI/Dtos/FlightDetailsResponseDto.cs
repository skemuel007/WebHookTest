namespace AirlineAPI.Dtos
{
    public class FlightDetailsResponseDto
    {
        public int Id { get; set; }
        public string FlightCode { get; set; } = default!;
        public decimal Price { get; set; }
    }
}
