using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
{
    public class DeleteWorkflowResponse : ApiResponse
    {
        [JsonPropertyName("is_deleted")]
        public bool IsDeleted { get; set; }
    }
}