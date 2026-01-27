using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.Common
{
    public class NotionErrorResponse
    {
        [JsonPropertyName("code")] 
        public string Code { get; set; }
        
        [JsonPropertyName("message")] 
        public string Message { get; set; }
    }
}