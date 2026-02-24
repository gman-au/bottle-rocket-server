using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Scans
{
    public class ArchiveScanResponse : ApiResponse
    {
        [JsonPropertyName("is_archived")]
        public bool IsArchived { get; set; }
    }
}