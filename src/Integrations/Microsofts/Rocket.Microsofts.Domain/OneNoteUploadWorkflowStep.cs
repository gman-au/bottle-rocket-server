using Rocket.Domain.Workflows;

namespace Rocket.Microsofts.Domain
{
    public record OneNoteUploadWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = MicrosoftDomainConstants.OneNoteUploadInputTypes;

        public override int OutputType { get; set; } = MicrosoftDomainConstants.OneNoteUploadOutputType;

        public override string StepName { get; set; } = "Upload note to OneNote";

        public override string StepCode { get; set; } = MicrosoftDomainConstants.OneNoteUploadWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = MicrosoftDomainConstants.ConnectorCode;

        public string SectionId { get; set; }
    }
}