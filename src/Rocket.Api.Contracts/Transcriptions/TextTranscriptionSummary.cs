using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Transcriptions
{
    public class TextTranscriptionSummary : TranscriptionSummary
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}