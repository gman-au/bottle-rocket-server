using System;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Transcriptions
{
    public class TranscriptionSummary : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("execution_id")]
        public string ExecutionId { get; set; }
        
        [JsonPropertyName("execution_step_name")]
        public string ExecutionStepName { get; set; }
        
        [JsonPropertyName("execution_exists")]
        public bool ExecutionExists { get; set; }
        
        [JsonPropertyName("transcription_date")]
        public DateTime TranscriptionDate { get; set; }
    }
}