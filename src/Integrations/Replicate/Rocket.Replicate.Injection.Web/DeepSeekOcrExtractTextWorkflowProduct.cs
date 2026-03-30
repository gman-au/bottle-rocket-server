using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Replicate.Domain;

namespace Rocket.Replicate.Injection.Web
{
    public class DeepSeekOcrExtractTextWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Extract text (lucataco/deepseek-ocr)";

        public string Description => "Extract text from image using Replicate (model: lucataco/deepseek-ocr)";

        public string[] Categories => [SkuConstants.TextRecognition];

        public string HrefBase => "/MyWorkflow/Replicate/DeepSeekOcr";

        public string ImagePath => "/img/replicate-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillLeaveYourServer;

        public string StepCode => ReplicateDomainConstants.DeepSeekOcrExtractTextWorkflowCode;
        
        public int[] InputTypes => ReplicateDomainConstants.ReplicateExtractInputTypes;

        public int OutputType => ReplicateDomainConstants.ReplicateExtractTextOutputType;
    }
}