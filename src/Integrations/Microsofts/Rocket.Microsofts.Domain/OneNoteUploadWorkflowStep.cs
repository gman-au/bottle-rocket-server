using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.Microsofts.Domain
{
    public record OneNoteUploadWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.Void;

        public override string StepName { get; set; } = "Upload note to OneNote";

        public override string RequiresConnectorCode { get; set; } = MicrosoftDomainConstants.ConnectorCode;

        public string SectionId { get; set; }
    }
}