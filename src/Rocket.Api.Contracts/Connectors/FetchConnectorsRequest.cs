using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Connectors
{
    public class FetchConnectorsRequest
    {
        [JsonPropertyName("start_index")]
        public int? StartIndex { get; set; }
        
        [JsonPropertyName("record_count")]
        public int? RecordCount { get; set; }
        
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}