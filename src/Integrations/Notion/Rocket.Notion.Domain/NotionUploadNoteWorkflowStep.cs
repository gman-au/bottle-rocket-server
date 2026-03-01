using Rocket.Domain.Workflows;

namespace Rocket.Notion.Domain
{
    public record NotionUploadNoteWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = NotionDomainConstants.NotionUploadNoteInputTypes;

        public override int OutputType { get; set; } = NotionDomainConstants.NotionUploadNoteOutputType;

        public override string StepName { get; set; } = "Upload note to Notion";

        public override string StepCode { get; set; } = NotionDomainConstants.NotionUploadNoteWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = NotionDomainConstants.ConnectorCode;

        public string ParentNoteId { get; set; }
    }
}