using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Batches
{
    public class ArchiveBatchResponse : ApiResponse
    {
        [JsonPropertyName("is_archived")]
        public bool IsArchived { get; set; }
    }
}