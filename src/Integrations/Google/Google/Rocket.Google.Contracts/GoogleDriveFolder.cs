using System.Text.Json.Serialization;

namespace Rocket.Google.Contracts
{
    public class GoogleDriveFolder
    {
        [JsonPropertyName("folder_id")]
        public string FolderId { get; set; }
        
        [JsonPropertyName("folder_name")]
        public string FolderName { get; set; }
    }
}