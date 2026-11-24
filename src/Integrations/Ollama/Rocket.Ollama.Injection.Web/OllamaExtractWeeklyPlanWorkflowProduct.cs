using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Ollama.Domain;

namespace Rocket.Ollama.Injection.Web
{
    public class OllamaExtractWeeklyPlanWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Extract weekly plan from image (Ollama)";

        public string Description => "Extract weekly plan information from an image using an Ollama model.";

        public string[] Categories => [SkuConstants.TextRecognition, SkuConstants.Planning, SkuConstants.AiMl];

        public string HrefBase => "/MyWorkflow/Ollama/ExtractWeeklyPlan";

        public string ImagePath => "/img/ollama-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataMightLeaveYourServer;

        public string StepCode => OllamaDomainConstants.OllamaExtractWeeklyPlanWorkflowCode;
        
        public int[] InputTypes => OllamaDomainConstants.OllamaExtractWeeklyPlanTaskInputTypes;

        public int OutputType => OllamaDomainConstants.OllamaExtractWeeklyPlanTaskOutputType;
    }
}