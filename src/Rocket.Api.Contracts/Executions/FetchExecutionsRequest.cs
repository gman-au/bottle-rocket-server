using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Executions
{
    public class FetchExecutionsRequest
    {
        [JsonPropertyName("start_index")]
        public int? StartIndex { get; set; }
        
        [JsonPropertyName("record_count")]
        public int? RecordCount { get; set; }

        [JsonPropertyName("workflow_id")]
        public string WorkflowId { get; set; }

        [JsonPropertyName("batch_id")]
        public string BatchId { get; set; }
    }
}