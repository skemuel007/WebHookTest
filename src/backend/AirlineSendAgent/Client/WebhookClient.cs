using AirlineSendAgent.Dtos;
using Serilog;
using System.Text.Json;

namespace AirlineSendAgent.Client
{
    public class WebhookClient : IWebhookClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        public WebhookClient(IHttpClientFactory httpClientFactory,
            ILogger logger) { 
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public async Task SendWebhookNotification(FlightDetailChangePayloadDto flightDetailChangePayloadDto)
        {
            var serializedPayload = JsonSerializer.Serialize(flightDetailChangePayloadDto);
            var httpClient = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, flightDetailChangePayloadDto.WebhookUri);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(serializedPayload);
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            try
            {
                using (var response = await httpClient.SendAsync(request))
                {
                    _logger.Information($"Successful sent webhook {serializedPayload}");
                    response.EnsureSuccessStatusCode(); ;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Unsucessful {ex.Message}");
            }
        }
    }
}
