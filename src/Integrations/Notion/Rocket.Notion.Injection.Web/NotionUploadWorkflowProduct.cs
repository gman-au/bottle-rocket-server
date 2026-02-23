using Rocket.Api.Contracts;
using Rocket.Interfaces;

namespace Rocket.Notion.Injection.Web
{
    public class NotionUploadWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Upload file to Notion";

        public string Description => "Upload a file (image, text, PDF) to a Notion workspace.";

        public string[] Categories => [SkuConstants.FileUpload];

        public string HrefBase => "/MyWorkflow/Notion";

        public string ImagePath => "/img/notion-logo.png";

        public bool DataLeavesYourServer => true;
    }
}