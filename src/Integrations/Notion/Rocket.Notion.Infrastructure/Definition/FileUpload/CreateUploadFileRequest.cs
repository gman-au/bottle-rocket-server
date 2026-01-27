using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.FileUpload
{
    public class CreateUploadFileRequest
    {
        [JsonPropertyName("mode")] 
        public string Mode { get; set; }
        
        [JsonPropertyName("filename")] 
        public string FileName { get; set; }
        
        [JsonPropertyName("content_type")] 
        public string ContentType { get; set; }
    }
}