using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition
{
    public class SearchResultProperties
    {
        [JsonPropertyName("title")] 
        public SearchResultProperty Title { get; set; }
    }
}