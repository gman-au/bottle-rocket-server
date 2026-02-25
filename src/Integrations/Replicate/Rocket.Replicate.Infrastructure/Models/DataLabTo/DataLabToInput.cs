using System.Text.Json.Serialization;
using Rocket.Replicate.Infrastructure.Definition;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo
{
    public class DataLabToInput : IReplicateInput
    {
        [JsonPropertyName("file")]
        public string File { get; set; }
        
        [JsonPropertyName("disable_image_extraction")]
        public bool DisableImageExtraction { get; set; }
        
        [JsonPropertyName("include_metadata")]
        public bool IncludeMetadata { get; set; }
        
        [JsonPropertyName("output_format")]
        public string OutputFormat { get; set; }
        
        [JsonPropertyName("page_schema")]
        public dynamic PageSchema { get; set; }
        
        [JsonPropertyName("use_llm")]
        public bool UseLlm { get; set; }
    }
}