using Rocket.Domain.Workflows;

namespace Rocket.Markdig.Domain
{
    public record ConvertToEpubWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = MarkdigDomainConstants.ConvertToEpubInputTypes;

        public override int OutputType { get; set; } = MarkdigDomainConstants.ConvertToEpubOutputType;

        public override string StepName
        {
            get => MergeFiles ? "Convert (merged) data to EPUB" : "Convert data to EPUB";
            set { }
        }

        public override string StepCode { get; set; } = MarkdigDomainConstants.ConvertToEpubWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = string.Empty;

        public bool MergeFiles { get; set; }
    }
}