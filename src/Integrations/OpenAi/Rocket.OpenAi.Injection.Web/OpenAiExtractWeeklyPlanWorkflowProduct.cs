using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.OpenAi.Domain;

namespace Rocket.OpenAi.Injection.Web
{
    public class OpenAiExtractWeeklyPlanWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Extract weekly plan from image (OpenAI)";

        public string Description => "Extract weekly plan information from an image using an OpenAI model.";

        public string[] Categories => [SkuConstants.TextRecognition, SkuConstants.Planning, SkuConstants.AiMl];

        public string HrefBase => "/MyWorkflow/OpenAi/ExtractWeeklyPlan";

        public string ImagePath => "/img/openai-logo.jpg";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillLeaveYourServer;

        public string StepCode => OpenAiDomainConstants.OpenAiExtractWeeklyPlanWorkflowCode;
        
        public int[] InputTypes => OpenAiDomainConstants.OpenAiExtractWeeklyPlanInputTypes;

        public int OutputType => OpenAiDomainConstants.OpenAiExtractWeeklyPlanOutputType;
    }
}