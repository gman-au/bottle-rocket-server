using Rocket.Api.Contracts;
using Rocket.Google.Domain;
using Rocket.Interfaces;

namespace Rocket.Google.Injection.Web
{
    public class GoogleDriveUploadWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Upload file to Google Drive";

        public string Description => "Upload a file (image, text, PDF) to a Google Drive folder.";

        public string[] Categories => [SkuConstants.FileUpload, SkuConstants.Google];

        public string HrefBase => "/MyWorkflow/GcpUpload";

        public string ImagePath => "/img/google-drive-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillLeaveYourServer;

        public string StepCode => GoogleDomainConstants.UploadFileWorkflowCode;
        
        public int[] InputTypes => GoogleDomainConstants.GoogleDriveUploadInputTypes;
        
        public int OutputType => GoogleDomainConstants.GoogleDriveUploadOutputType;
    }
}