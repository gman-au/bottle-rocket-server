using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.FileUpload
{
    public class CreateUploadFileResponse
    {
        [JsonPropertyName("id")] 
        public string Id { get; set; }
        
        [JsonPropertyName("object")] 
        public string Object { get; set; }
        
        [JsonPropertyName("content_type")] 
        public string ContentType { get; set; }
    }
}