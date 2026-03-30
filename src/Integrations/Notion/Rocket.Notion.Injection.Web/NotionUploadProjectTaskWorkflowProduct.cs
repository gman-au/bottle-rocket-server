using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Notion.Domain;

namespace Rocket.Notion.Injection.Web
{
    public class NotionUploadProjectTaskWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Upload project task to Notion";

        public string Description => "Upload a project task data to a Notion data source.";

        public string[] Categories => [SkuConstants.FileUpload, SkuConstants.ProjectManagement];

        public string HrefBase => "/MyWorkflow/Notion/ProjectTask";

        public string ImagePath => "/img/notion-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillLeaveYourServer;

        public string StepCode => NotionDomainConstants.NotionUploadProjectTaskWorkflowCode;
        
        public int[] InputTypes { get; set; } = NotionDomainConstants.NotionUploadProjectTaskInputTypes;

        public int OutputType => NotionDomainConstants.NotionUploadProjectTaskOutputType;
    }
}