using System.Text.Json.Serialization;
using Rocket.Replicate.Infrastructure.Definition;

namespace Rocket.Replicate.Infrastructure.Models.DeepSeekOcr
{
    public class DeepSeekOcrInput : IReplicateInput
    {
        [JsonPropertyName("image")]
        public string Image { get; set; }
        
        [JsonPropertyName("resolution_size")]
        public bool ResolutionSize { get; set; }
        
        [JsonPropertyName("task_type")]
        public bool TaskType { get; set; }
    }
}