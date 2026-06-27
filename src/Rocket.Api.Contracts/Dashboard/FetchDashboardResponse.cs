using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Dashboard
{
    public class FetchDashboardResponse : ApiResponse
    {
        [JsonPropertyName("scans")]
        public ScansResponse Scans { get; set; }
        
        [JsonPropertyName("storage")]
        public StorageResponse Storage { get; set; }
        
        [JsonPropertyName("lifecycle")]
        public LifecycleResponse Lifecycle { get; set; }
    }
}