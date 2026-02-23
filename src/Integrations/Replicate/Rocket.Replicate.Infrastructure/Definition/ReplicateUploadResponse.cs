using System.Text.Json.Serialization;

namespace Rocket.Replicate.Infrastructure.Definition
{
    public class ReplicateUploadResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("urls")]
        public UploadUrls Urls { get; set; }
    }
}