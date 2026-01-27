using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.Common
{
    public class NotionProperties
    {
        [JsonPropertyName("title")] 
        public NotionPropertySet Title { get; set; }
    }
}