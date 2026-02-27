using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.Notion.Domain
{
    public record NotionUploadProjectTaskWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } =
        [
            (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData
        ];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.Void;

        public override string StepName { get; set; } = "Upload project task data to Notion";

        public override string StepCode { get; set; } = NotionDomainConstants.NotionUploadNoteWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = NotionDomainConstants.ConnectorCode;

        public string DataSourceId { get; set; }
        
        public string ProjectCodeColumn { get; set; }
        
        public string TaskColumn { get; set; }
        
        public string DueDateColumn { get; set; }
        
        public string EstTimeColumn { get; set; }
    }
}