using AirlineAPI.Dtos;
using AirlineAPI.Models;
using AutoMapper;

namespace AirlineAPI.Profiles
{
    public class FlightDetailProfile : Profile
    {
        public FlightDetailProfile()
        {
            CreateMap<FlightDetail, FlightDetailsForCreateDto>().ReverseMap();
            CreateMap<FlightDetail, FlightDetailsForUpdateDto>().ReverseMap();
            CreateMap<FlightDetail, FlightDetailsResponseDto>().ReverseMap();
        }
    }
}
