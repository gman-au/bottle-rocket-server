using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Batches
{
    public class OpenBatchResponse : ApiResponse
    {        
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}