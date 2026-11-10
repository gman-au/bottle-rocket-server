using Rocket.Domain.Workflows;

namespace Rocket.QuestPdf.Domain
{
    public record ConvertToPdfWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = QuestPdfDomainConstants.ConvertToPdfInputTypes;

        public override int OutputType { get; set; } = QuestPdfDomainConstants.ConvertToPdfOutputType;

        public override string StepName
        {
            get => MergeFiles ? "Convert (merged) data to PDF" : "Convert data to PDF";
            set { }
        }

        public override string StepCode { get; set; } = QuestPdfDomainConstants.ConvertToPdfWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = string.Empty;

        public bool MergeFiles { get; set; }
    }
}