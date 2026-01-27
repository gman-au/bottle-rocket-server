using System.Text.Json.Serialization;
using Rocket.Notion.Infrastructure.Definition.Common;

namespace Rocket.Notion.Infrastructure.Definition.FileUpload
{
    public class NotionImageFileUploadBlock : NotionBlock
    {
        [JsonPropertyName("object")]
        public string Object { get; set; } = "block";
        
        [JsonPropertyName("type")]
        public string Type { get; set; } = "image";
        
        [JsonPropertyName("image")]
        public NotionUploadedImageFile Image { get; set; }
    }
}