using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.Common
{
    public abstract class NotionBlock
    {
        [JsonPropertyName("object")]
        public string Object { get; set; } = "block";
        
        public abstract string Type { get; set; }
    }
}