using System;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Scans
{
    public class TranscriptionSpecifics : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("execution_id")]
        public string ExecutionId { get; set; }
        
        [JsonPropertyName("execution_name")]
        public string ExecutionName { get; set; }
        
        [JsonPropertyName("execution_exists")]
        public bool ExecutionExists { get; set; }
        
        [JsonPropertyName("transcription_date")]
        public DateTime TranscriptionDate { get; set; }
        
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}