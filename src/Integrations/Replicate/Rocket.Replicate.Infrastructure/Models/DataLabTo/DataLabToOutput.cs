using System.Text.Json.Serialization;
using Rocket.Replicate.Infrastructure.Definition;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo
{
    public class DataLabToOutput : IReplicateOutput
    {
        [JsonPropertyName("markdown")]
        public string Markdown { get; set; }
        
        [JsonPropertyName("extraction_schema_json")]
        public string ExtractionSchemaJson { get; set; }
    }
}