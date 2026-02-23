using Rocket.Api.Contracts;
using Rocket.Gcp.Domain;
using Rocket.Interfaces;

namespace Rocket.Gcp.Injection.Web
{
    public class GcpExtractWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Extract text from image (Google Vision API)";

        public string Description => "Extract text from an image using Google Vision API.";

        public string[] Categories => [SkuConstants.TextRecognition];

        public string HrefBase => "/MyWorkflow/GcpExtract";

        public string ImagePath => "/img/gcp-logo.png";

        public bool DataLeavesYourServer => true;

        public string StepCode => GcpDomainConstants.ExtractTextWorkflowCode;
    }
}