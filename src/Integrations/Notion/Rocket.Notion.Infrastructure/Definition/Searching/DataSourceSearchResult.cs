using System.Collections.Generic;
using System.Text.Json.Serialization;
using Rocket.Notion.Infrastructure.Definition.Common;

namespace Rocket.Notion.Infrastructure.Definition.Searching
{
    public class DataSourceSearchResult
    {
        [JsonPropertyName("object")] 
        public string Object { get; set; }
        
        [JsonPropertyName("id")] 
        public string Id { get; set; }
        
        [JsonPropertyName("title")] 
        public IEnumerable<NotionProperty> Title { get; set; }
        
        [JsonPropertyName("properties")] 
        public Dictionary<string, DataSourceProperty> Properties { get; set; }
    }
}