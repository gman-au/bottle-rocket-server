using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition
{
    public class PageSearchResponse
    {
        [JsonPropertyName("object")] 
        public string Object { get; set; }
        
        [JsonPropertyName("results")] 
        public IEnumerable<SearchResult> Results { get; set; }
    }
}