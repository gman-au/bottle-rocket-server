using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.Common
{
    public class NotionRichTextProperty
    {
        [JsonPropertyName("rich_text")] 
        public IEnumerable<NotionProperty> RichText { get; set; }
    }
}