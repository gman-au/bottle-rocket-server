using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class EmailFileAttachmentStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("target_email_address")]
        public string TargetEmailAddress { get; set; }
    }
}