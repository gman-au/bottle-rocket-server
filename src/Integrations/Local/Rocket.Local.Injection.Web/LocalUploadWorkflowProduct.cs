using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Local.Domain;

namespace Rocket.Local.Injection.Web
{
    public class LocalUploadWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Upload file to server folder";

        public string Description => "Upload a file (image, text, PDF) to a server folder.";

        public string[] Categories => [SkuConstants.FileUpload];

        public string HrefBase => "/MyWorkflow/LocalUpload";

        public string ImagePath => "/img/bottle-rocket-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillNotLeaveYourServer;

        public string StepCode => LocalDomainConstants.LocalUploadWorkflowCode;
        
        public int[] InputTypes => LocalDomainConstants.LocalUploadInputTypes;

        public int OutputType => LocalDomainConstants.LocalUploadOutputType;
    }
}