using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.Searching
{
    public class PageSearchRequest
    {
        [JsonPropertyName("filter")] 
        public SearchFilter Filter { get; set; }
    }
}