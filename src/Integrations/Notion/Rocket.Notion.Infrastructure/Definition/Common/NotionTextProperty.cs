using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.Common
{
    public class NotionTextProperty
    {
        [JsonPropertyName("content")] 
        public string Content { get; set; }
        
        [JsonPropertyName("link")] 
        public string Link { get; set; }
    }
}