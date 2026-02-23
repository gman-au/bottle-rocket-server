using System.Text.Json.Serialization;

namespace Rocket.Replicate.Infrastructure.Definition
{
    public class UploadUrls
    {
        [JsonPropertyName("get")]
        public string GetUrl { get; set; }
    }
}