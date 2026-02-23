using System.Text.Json.Serialization;
using Rocket.Replicate.Infrastructure.Definition;

namespace Rocket.Replicate.Infrastructure.Models
{
    public class DataLabToInput : IReplicateInput
    {
        [JsonPropertyName("file")]
        public string File { get; set; }
        
        [JsonPropertyName("disable_image_extraction")]
        public bool DisableImageExtraction { get; set; }
    }
}