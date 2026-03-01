using Rocket.Api.Contracts;
using Rocket.Dropbox.Domain;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Injection.Web
{
    public class DropboxUploadWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Upload file to Dropbox";

        public string Description => "Upload a file (image, text, PDF) to a Dropbox folder.";

        public string[] Categories => [SkuConstants.FileUpload];

        public string HrefBase => "/MyWorkflow/Dropbox";

        public string ImagePath => "/img/dropbox-logo.webp";

        public bool DataLeavesYourServer => true;

        public string StepCode => DropboxDomainConstants.UploadWorkflowCode;
        
        public int[] InputTypes => DropboxDomainConstants.DropboxUploadInputTypes;

        public int OutputType => DropboxDomainConstants.DropboxUploadOutputType;
    }
}