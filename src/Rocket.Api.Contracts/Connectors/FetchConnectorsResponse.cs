using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Connectors
{
    public class FetchConnectorsResponse : ApiResponse
    {
        [JsonPropertyName("connectors")]
        public IEnumerable<ConnectorSummary> Connectors { get; set; }
        
        [JsonPropertyName("total_records")]
        public int TotalRecords { get; set; }
    }
}