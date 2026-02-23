using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Injection.Web
{
    public class OneDriveUploadWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Upload file to OneDrive";

        public string Description => "Upload a file (image, text, PDF) to a Microsoft OneDrive folder.";

        public string[] Categories => [SkuConstants.FileUpload];

        public string HrefBase => "/MyWorkflow/OneDrive";

        public string ImagePath => "/img/onedrive-logo.png";

        public bool DataLeavesYourServer => true;

        public string StepCode => MicrosoftDomainConstants.OneDriveUploadWorkflowCode;
    }
}