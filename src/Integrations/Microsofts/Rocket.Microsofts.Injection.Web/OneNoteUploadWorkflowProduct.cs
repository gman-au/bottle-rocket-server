using Rocket.Api.Contracts;
using Rocket.Interfaces;

namespace Rocket.Microsofts.Injection.Web
{
    public class OneNoteUploadWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Upload file to OneNote";

        public string Description => "Upload a file (image, text, PDF) to a Microsoft OneNote folder.";

        public string[] Categories => [SkuConstants.FileUpload];

        public string HrefBase => "/MyWorkflow/OneNote";

        public string ImagePath => "/img/onenote-logo.png";

        public bool DataLeavesYourServer => true;
    }
}