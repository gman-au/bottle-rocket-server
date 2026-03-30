using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Injection.Web
{
    public class OneNoteUploadWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Upload file to OneNote";

        public string Description => "Upload a file (image, text, PDF) to a Microsoft OneNote folder.";

        public string[] Categories => [SkuConstants.FileUpload, SkuConstants.Microsoft];

        public string HrefBase => "/MyWorkflow/OneNote";

        public string ImagePath => "/img/onenote-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillLeaveYourServer;

        public string StepCode => MicrosoftDomainConstants.OneNoteUploadWorkflowCode;
        
        public int[] InputTypes => MicrosoftDomainConstants.OneNoteUploadInputTypes;

        public int OutputType => MicrosoftDomainConstants.OneNoteUploadOutputType;
    }
}