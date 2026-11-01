using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Sessions
{
    public class MySessionsResponse : ApiResponse
    {
        [JsonPropertyName("sessions")]
        public IEnumerable<TokenSessionSpecifics> Sessions { get; set; }
        
        [JsonPropertyName("total_records")]
        public int TotalRecords { get; set; }
    }
}