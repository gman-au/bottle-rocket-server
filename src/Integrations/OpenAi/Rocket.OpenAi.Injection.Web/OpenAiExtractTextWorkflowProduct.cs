using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.OpenAi.Domain;

namespace Rocket.OpenAi.Injection.Web
{
    public class OpenAiExtractTextWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Extract raw text from image (OpenAI)";

        public string Description => "Extract raw text from an image using an OpenAI model.";

        public string[] Categories => [SkuConstants.TextRecognition, SkuConstants.AiMl];

        public string HrefBase => "/MyWorkflow/OpenAi/ExtractText";

        public string ImagePath => "/img/openai-logo.jpg";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillLeaveYourServer;

        public string StepCode => OpenAiDomainConstants.OpenAiExtractTextWorkflowCode;
        
        public int[] InputTypes => OpenAiDomainConstants.OpenAiExtractTextInputTypes;

        public int OutputType => OpenAiDomainConstants.OpenAiExtractTextOutputType;
    }
}