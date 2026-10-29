using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.TokenSessions
{
    public class MySessionsResponse : ApiResponse
    {
        [JsonPropertyName("sessions")]
        public IEnumerable<TokenSessionSpecifics> Sessions { get; set; }
    }
}