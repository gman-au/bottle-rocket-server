using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.Searching
{
    public class PageSearchResponse
    {
        [JsonPropertyName("object")] 
        public string Object { get; set; }
        
        [JsonPropertyName("results")] 
        public IEnumerable<PageSearchResult> Results { get; set; }
    }
}