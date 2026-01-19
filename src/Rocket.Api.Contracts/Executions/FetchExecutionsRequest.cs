using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Executions
{
    public class FetchExecutionsRequest
    {
        [JsonPropertyName("start_index")]
        public int StartIndex { get; set; }
        
        [JsonPropertyName("record_count")]
        public int RecordCount { get; set; }
    }
}