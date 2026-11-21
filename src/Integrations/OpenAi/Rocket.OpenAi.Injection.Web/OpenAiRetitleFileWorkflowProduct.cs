using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.OpenAi.Domain;

namespace Rocket.OpenAi.Injection.Web
{
    public class OpenAiRetitleFileWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Auto-title file (OpenAI)";

        public string Description => "Re-title a document file using an OpenAI model.";

        public string[] Categories => [SkuConstants.FileRetitling];

        public string HrefBase => "/MyWorkflow/OpenAi/RetitleFile";

        public string ImagePath => "/img/openai-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillLeaveYourServer;

        public string StepCode => OpenAiDomainConstants.OpenAiRetitleFileWorkflowCode;
        
        public int[] InputTypes => OpenAiDomainConstants.OpenAiRetitleFileInputTypes;

        public int OutputType => OpenAiDomainConstants.OpenAiRetitleFileOutputType;
    }
}