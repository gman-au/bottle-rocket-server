using System.Text.Json.Serialization;
using Rocket.Replicate.Infrastructure.Definition;

namespace Rocket.Replicate.Infrastructure.Models.Gpt
{
    public class GptSummarizeInput : IReplicateInput
    {
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }
        
        [JsonPropertyName("system_prompt")]
        public string SystemPrompt { get; set; }
    }
}