using Rocket.Domain.Workflows;

namespace Rocket.Notion.Domain
{
    public record NotionUploadProjectTaskWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = NotionDomainConstants.NotionUploadProjectTaskInputTypes;

        public override int OutputType { get; set; } = NotionDomainConstants.NotionUploadProjectTaskOutputType;

        public override string StepName { get; set; } = "Upload project task data to Notion";

        public override string StepCode { get; set; } = NotionDomainConstants.NotionUploadProjectTaskWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = NotionDomainConstants.ConnectorCode;

        public string DataSourceId { get; set; }
        
        public string ProjectCodeColumn { get; set; }
        
        public string TaskColumn { get; set; }
        
        public string DueDateColumn { get; set; }
        
        public string EstTimeColumn { get; set; }
    }
}