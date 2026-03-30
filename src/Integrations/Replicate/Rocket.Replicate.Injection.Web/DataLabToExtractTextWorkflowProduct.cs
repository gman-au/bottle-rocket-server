using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Replicate.Domain;

namespace Rocket.Replicate.Injection.Web
{
    public class DataLabToExtractTextWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Extract text (datalab-to/marker)";

        public string Description => "Extract text from image using Replicate (model: datalab-to/marker)";

        public string[] Categories => [SkuConstants.TextRecognition];

        public string HrefBase => "/MyWorkflow/Replicate/DataLabTo/Text";

        public string ImagePath => "/img/replicate-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillLeaveYourServer;

        public string StepCode => ReplicateDomainConstants.DataLabToExtractTextWorkflowCode;
        
        public int[] InputTypes => ReplicateDomainConstants.ReplicateExtractInputTypes;

        public int OutputType => ReplicateDomainConstants.ReplicateExtractTextOutputType;
    }
}