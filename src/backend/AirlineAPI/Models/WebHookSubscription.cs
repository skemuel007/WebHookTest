using System.ComponentModel.DataAnnotations;

namespace AirlineAPI.Models
{
    public class WebHookSubscription
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string WebHookUri { get; set; }
        [Required]
        public string Secret { get; set; }
        [Required]
        public string WebHookType { get; set; }
        [Required]
        public string WebHookPublisher { get; set; }
    }
}
