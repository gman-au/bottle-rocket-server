using Rocket.Domain.Executions;

namespace Rocket.Notion.Domain
{
    public record NotionUploadProjectTaskExecutionStep : BaseExecutionStep
    {
        public string DataSourceId { get; set; }

        public string ProjectCodeColumn { get; set; }

        public string TaskColumn { get; set; }

        public string DueDateColumn { get; set; }

        public string EstTimeColumn { get; set; }
    }
}