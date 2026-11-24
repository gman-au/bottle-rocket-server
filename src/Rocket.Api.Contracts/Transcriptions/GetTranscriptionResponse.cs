using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Transcriptions
{
    public class GetTranscriptionResponse : ApiResponse
    {
        [JsonPropertyName("transcription")]
        public TranscriptionSummary Transcription { get; set; }
    }
}