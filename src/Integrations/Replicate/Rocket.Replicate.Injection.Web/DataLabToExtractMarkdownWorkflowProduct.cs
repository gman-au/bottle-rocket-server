using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Replicate.Domain;

namespace Rocket.Replicate.Injection.Web
{
    public class DataLabToExtractMarkdownWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Extract markdown (datalab-to/marker)";

        public string Description => "Extract markdown from image using Replicate (model: datalab-to/marker)";

        public string[] Categories => [SkuConstants.TextRecognition];

        public string HrefBase => "/MyWorkflow/Replicate/DataLabTo/Markdown";

        public string ImagePath => "/img/replicate-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillLeaveYourServer;

        public string StepCode => ReplicateDomainConstants.DataLabToExtractMarkdownWorkflowCode;
        
        public int[] InputTypes => ReplicateDomainConstants.ReplicateExtractInputTypes;

        public int OutputType => ReplicateDomainConstants.ReplicateExtractMarkdownOutputType;
    }
}