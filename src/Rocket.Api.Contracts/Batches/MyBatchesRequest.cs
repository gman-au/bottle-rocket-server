using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Batches
{
    public class MyBatchesRequest
    {
        [JsonPropertyName("start_index")]
        public int StartIndex { get; set; }
        
        [JsonPropertyName("record_count")]
        public int RecordCount { get; set; }
        
        [JsonPropertyName("search_term")]
        public string SearchTerm { get; set; }
    }
}