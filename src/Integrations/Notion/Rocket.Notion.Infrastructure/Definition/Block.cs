using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition
{
    public class Block
    {
        [JsonPropertyName("type")] 
        public string Type { get; set; }
        
        [JsonPropertyName("text")] 
        public BlockText Text { get; set; }
    }
}