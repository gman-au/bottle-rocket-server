using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.Common
{
    public class NotionPropertySet
    {
        [JsonPropertyName("title")] 
        public IEnumerable<NotionProperty> Title { get; set; }
    }
}