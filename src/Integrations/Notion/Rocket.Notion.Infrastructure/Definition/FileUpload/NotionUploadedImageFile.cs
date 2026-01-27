using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.FileUpload
{
    public class NotionUploadedImageFile
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "file_upload";
        
        [JsonPropertyName("file_upload")]
        public FileUpload FileUpload { get; set; }
    }
}