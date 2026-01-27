using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition
{
    public class PageSearchRequest
    {
        [JsonPropertyName("filter")] 
        public SearchFilter Filter { get; set; }
    }
}