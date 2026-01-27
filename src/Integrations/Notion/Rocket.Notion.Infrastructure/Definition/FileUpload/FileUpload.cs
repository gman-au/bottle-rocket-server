using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.FileUpload
{
    public class FileUpload
    {
        [JsonPropertyName("id")] 
        public string Id { get; set; }
    }
}