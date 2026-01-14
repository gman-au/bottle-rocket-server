using System.Collections.Generic;
using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Connectors;

namespace Rocket.Api.Contracts
{
    public class FetchConnectorsResponse : ApiResponse
    {
        [JsonPropertyName("connectors")]
        public IEnumerable<ConnectorItem> Connectors { get; set; }
        
        [JsonPropertyName("total_records")]
        public int TotalRecords { get; set; }
    }
}