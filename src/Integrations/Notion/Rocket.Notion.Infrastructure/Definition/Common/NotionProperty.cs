using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.Common
{
    public class NotionProperty
    {
        [JsonPropertyName("type")] 
        public string Type { get; set; }
        
        [JsonPropertyName("text")] 
        public NotionTextProperty Text { get; set; }
    }
}