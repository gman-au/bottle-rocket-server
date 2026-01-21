using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Executions
{
    public class CreateExecutionResponse : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}