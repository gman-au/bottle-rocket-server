using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Executions
{
    public class ExecutionStepSummary : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("connector_id")]
        public string ConnectorId { get; set; }

        [JsonPropertyName("input_type")]
        public int InputType { get; set; }

        [JsonPropertyName("input_type_name")]
        public string InputTypeName { get; set; }

        [JsonPropertyName("output_type")]
        public int OutputType { get; set; }

        [JsonPropertyName("output_type_name")]
        public string OutputTypeName { get; set; }
        
        [JsonPropertyName("run_date")]
        public DateTime? RunDate { get; set; }

        [JsonPropertyName("step_name")]
        public string StepName { get; set; }

        [JsonPropertyName("execution_status")]
        public int? ExecutionStatus { get; set; }

        [JsonPropertyName("child_steps")]
        public IEnumerable<ExecutionStepSummary> ChildSteps { get; set; }
    }
}