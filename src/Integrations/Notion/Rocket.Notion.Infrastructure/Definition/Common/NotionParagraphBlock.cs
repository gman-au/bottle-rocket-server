using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.Common
{
    public class NotionParagraphBlock : NotionBlock
    {
        [JsonPropertyName("object")]
        public string Object { get; set; } = "block";
        
        [JsonPropertyName("type")]
        public string Type { get; set; } = "paragraph";
        
        [JsonPropertyName("paragraph")]
        public NotionParagraph Paragraph { get; set; }
    }
}