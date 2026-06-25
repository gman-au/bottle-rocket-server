using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Dashboard
{
    public class StorageResponse
    {
        [JsonPropertyName("used_storage_bytes")]
        public long? UsedStorageBytes { get; set; }
        
        [JsonPropertyName("used_storage_friendly")]
        public string UsedStorageFriendly { get; set; }
        
        [JsonPropertyName("available_storage_bytes")]
        public long? AvailableStorageBytes { get; set; }
        
        [JsonPropertyName("available_storage_friendly")]
        public string AvailableStorageFriendly { get; set; }
    }
}