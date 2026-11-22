using Rocket.Domain.Workflows;

namespace Rocket.Local.Domain
{
    public record LocalRetitleFileWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = LocalDomainConstants.LocalRetitleInputTypes;

        public override int OutputType { get; set; } = LocalDomainConstants.LocalRetitleFileOutputType;

        public override string StepName { get; set; } = "Suggest a title for the file based on its contents";

        public override string StepCode { get; set; } = LocalDomainConstants.LocalRetitleWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = string.Empty;
    }
}