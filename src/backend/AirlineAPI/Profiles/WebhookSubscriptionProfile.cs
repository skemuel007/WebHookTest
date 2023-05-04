using AirlineAPI.Dtos;
using AirlineAPI.Models;
using AutoMapper;

namespace AirlineAPI.Profiles
{
    public class WebhookSubscriptionProfile : Profile
    {
        public WebhookSubscriptionProfile() {
            CreateMap<WebHookSubscriptionCreateDto, WebHookSubscription>()
                .ForMember(x => x.WebHookType, opt => opt.MapFrom(x => x.WebhookType))
                .ForMember(x=> x.WebHookUri, opt => opt.MapFrom(x => x.WebhookUri))
                .ReverseMap();

            CreateMap<WebHookSubscriptionResponsDto, WebHookSubscription>()
                .ForMember(x => x.WebHookType, opt => opt.MapFrom(x => x.WebhookType))
                .ForMember(x => x.WebHookUri, opt => opt.MapFrom(x => x.WebhookUri))
                .ForMember(x => x.WebHookPublisher, opt => opt.MapFrom(x => x.WebhookPublisher))
                .ReverseMap();
        }
    }
}
