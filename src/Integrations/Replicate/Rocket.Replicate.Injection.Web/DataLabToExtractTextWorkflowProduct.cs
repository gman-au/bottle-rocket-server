using Rocket.Api.Contracts;
using Rocket.Interfaces;

namespace Rocket.Replicate.Injection.Web
{
    public class DataLabToExtractTextWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Extract text (datalab-to/marker)";

        public string Description => "Extract text from image using Replicate (model: datalab-to/marker)";

        public string[] Categories => [SkuConstants.TextRecognition];

        public string HrefBase => "/MyWorkflow/Replicate/DataLabTo";

        public string ImagePath => "/img/replicate-logo.png";

        public bool DataLeavesYourServer => true;
    }
}