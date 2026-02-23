using Rocket.Api.Contracts;
using Rocket.Interfaces;

namespace Rocket.Google.Injection.Web
{
    public class GoogleDriveUploadWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Upload file to Google Drive";

        public string Description => "Upload a file (image, text, PDF) to a Google Drive folder.";

        public string[] Categories => [SkuConstants.FileUpload];

        public string HrefBase => "/MyWorkflow/GcpUpload";

        public string ImagePath => "/img/google-drive-logo.png";

        public bool DataLeavesYourServer => true;
    }
}