using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Dashboard
{
    public class StorageResponse
    {
        [JsonPropertyName("used_storage_mb")]
        public long? UsedStorageMb { get; set; }
        
        [JsonPropertyName("available_storage_mb")]
        public long? AvailableStorageMb { get; set; }
    }
}