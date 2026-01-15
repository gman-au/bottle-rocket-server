using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class EmailFileAttachmentStep : WorkflowStepSummary
    {
        [JsonPropertyName("target_email_address")]
        public string TargetEmailAddress { get; set; }
    }
}