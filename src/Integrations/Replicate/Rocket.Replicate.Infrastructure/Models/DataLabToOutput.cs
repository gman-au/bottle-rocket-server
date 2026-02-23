using System.Text.Json.Serialization;
using Rocket.Replicate.Infrastructure.Definition;

namespace Rocket.Replicate.Infrastructure.Models
{
    public class DataLabToOutput : IReplicateOutput
    {
        [JsonPropertyName("markdown")]
        public string Markdown { get; set; }
    }
}