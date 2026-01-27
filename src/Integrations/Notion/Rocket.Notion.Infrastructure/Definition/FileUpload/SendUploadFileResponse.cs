using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.FileUpload
{
    public class SendUploadFileResponse
    {
        [JsonPropertyName("id")] 
        public string Id { get; set; }
        
        [JsonPropertyName("object")] 
        public string Object { get; set; }
    }
}