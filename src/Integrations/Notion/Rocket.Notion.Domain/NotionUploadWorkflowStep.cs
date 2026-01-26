using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.Notion.Domain
{
    public record NotionUploadWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.Void;

        public override string StepName { get; set; } = "Upload note to Notion";

        public override string RequiresConnectorCode { get; set; } = NotionDomainConstants.ConnectorCode;

        public string ParentNoteId { get; set; }
    }
}