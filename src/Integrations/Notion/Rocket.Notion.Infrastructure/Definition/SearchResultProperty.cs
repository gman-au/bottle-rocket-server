using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition
{
    public class SearchResultProperty
    {
        [JsonPropertyName("id")] 
        public string Id { get; set; }
        
        [JsonPropertyName("title")] 
        public IEnumerable<Block> Title { get; set; }
    }
}