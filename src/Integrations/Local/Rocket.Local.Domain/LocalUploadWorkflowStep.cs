using Rocket.Domain.Workflows;

namespace Rocket.Local.Domain
{
    public record LocalUploadWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = LocalDomainConstants.LocalUploadInputTypes;

        public override int OutputType { get; set; } = LocalDomainConstants.LocalUploadOutputType;

        public override string StepName { get; set; } = "Upload note to local (mounted) server path";

        public override string StepCode { get; set; } = LocalDomainConstants.LocalUploadWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = string.Empty;

        public string UploadPath { get; set; }
    }
}