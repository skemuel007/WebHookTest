﻿using System.ComponentModel.DataAnnotations;

namespace AirlineSendAgent.Dtos
{
    public class FlightDetailChangePayloadDto
    {
        public string WebhookUri { get; set; }
        public string FlightCode { get; set; }
        public decimal OldPrice { get; set; }
        public string Secret { get; set; }
        public string Publisher { get; set; }
        public decimal NewPrice { get; set; }
        public string WebhookType { get; set; }

    }
}
