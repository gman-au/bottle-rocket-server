using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Executions
{
    public class DeleteExecutionResponse : ApiResponse
    {
        [JsonPropertyName("is_deleted")]
        public bool IsDeleted { get; set; }
    }
}