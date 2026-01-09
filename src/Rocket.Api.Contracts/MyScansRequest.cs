using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
{
    public class MyScansRequest
    {
        [JsonPropertyName("start_index")]
        public int StartIndex { get; set; }
        
        [JsonPropertyName("record_count")]
        public int RecordCount { get; set; }
    }
}