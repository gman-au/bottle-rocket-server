using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Replicate.Domain;

namespace Rocket.Replicate.Injection.Web
{
    public class GptRetitleFileWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Auto-title file (GPT)";

        public string Description => "Re-title a document file using Replicate (model: gpt-5.2).";

        public string[] Categories => [SkuConstants.FileRetitling, SkuConstants.AiMl];

        public string HrefBase => "/MyWorkflow/Replicate/Gpt/RetitleFile";

        public string ImagePath => "/img/replicate-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillLeaveYourServer;

        public string StepCode => ReplicateDomainConstants.GptRetitleFileWorkflowCode;
        
        public int[] InputTypes => ReplicateDomainConstants.ReplicateRetitleFileInputTypes;

        public int OutputType => ReplicateDomainConstants.ReplicateRetitleFileOutputType;
    }
}