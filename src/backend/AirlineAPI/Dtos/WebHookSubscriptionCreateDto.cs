using System.ComponentModel.DataAnnotations;

namespace AirlineAPI.Dtos
{
    public class WebHookSubscriptionCreateDto
    {
        [Required]
        public string WebhookUri { get; set; }
        [Required]
        public string WebhookType { get; set; }
    }
}
