using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Notion.Contracts
{
    public class NotionUploadProjectTaskWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("data_source_id")]
        public string DataSourceId { get; set; }
        
        [JsonPropertyName("project_code_column")]
        public string ProjectCodeColumn { get; set; }
        
        [JsonPropertyName("task_column")]
        public string TaskColumn { get; set; }
        
        [JsonPropertyName("due_date_column")]
        public string DueDateColumn { get; set; }
        
        [JsonPropertyName("est_time_column")]
        public string EstTimeColumn { get; set; }
    }
}