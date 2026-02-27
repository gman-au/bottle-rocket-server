using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.Searching
{
    public class DataSourceSearchResponse
    {
        [JsonPropertyName("object")] 
        public string Object { get; set; }
        
        [JsonPropertyName("results")] 
        public IEnumerable<DataSourceSearchResult> Results { get; set; }
    }
}