using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    // [JsonDerivedType(typeof(ConnectorDetail), typeDiscriminator: "base")]
    public class WorkflowStepSummary
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("connection_id")]
        public string ConnectionId { get; set; }

        [JsonPropertyName("input_type")]
        public int InputType { get; set; }

        [JsonPropertyName("input_type_name")]
        public string InputTypeName { get; set; }

        [JsonPropertyName("output_type")]
        public int OutputType { get; set; }

        [JsonPropertyName("output_type_name")]
        public string OutputTypeName { get; set; }

        [JsonPropertyName("step_name")]
        public string StepName { get; set; }

        [JsonPropertyName("child_steps")]
        public IEnumerable<WorkflowStepSummary> ChildSteps { get; set; }
    }
}