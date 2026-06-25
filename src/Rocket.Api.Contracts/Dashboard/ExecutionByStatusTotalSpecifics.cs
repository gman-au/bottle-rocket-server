using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Dashboard
{
    public class ExecutionByStatusTotalSpecifics
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        
        [JsonPropertyName("executions")]
        public int Executions { get; set; }
    }
}