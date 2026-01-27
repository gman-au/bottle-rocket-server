using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.Common
{
    public class NotionParagraphBlock : NotionBlock
    {
        [JsonPropertyName("type")]
        public override string Type { get; set; } = "paragraph";
        
        [JsonPropertyName("rich_text")] 
        public IEnumerable<NotionProperty> RichText { get; set; }
    }
}