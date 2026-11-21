using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.OpenAi.Domain;

namespace Rocket.OpenAi.Injection.Web
{
    public class OpenAiExtractProjectWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Extract project tasks from image (OpenAI)";

        public string Description => "Extract project task information from an image using an OpenAI model.";

        public string[] Categories => [SkuConstants.TextRecognition, SkuConstants.ProjectManagement];

        public string HrefBase => "/MyWorkflow/OpenAi/ExtractProject";

        public string ImagePath => "/img/openai-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillLeaveYourServer;

        public string StepCode => OpenAiDomainConstants.OpenAiExtractProjectTasksWorkflowCode;
        
        public int[] InputTypes => OpenAiDomainConstants.OpenAiExtractProjectTaskInputTypes;

        public int OutputType => OpenAiDomainConstants.OpenAiExtractProjectTaskOutputType;
    }
}